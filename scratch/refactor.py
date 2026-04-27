import os
import re

directory = r"D:\My projects\PrimeFit\src\PrimeFit.Application\Features"

for root, _, files in os.walk(directory):
    for file in files:
        if file.endswith("Handler.cs"):
            handler_path = os.path.join(root, file)
            with open(handler_path, 'r', encoding='utf-8') as f:
                content = f.read()

            if "IBranchAuthorizationService" not in content:
                continue

            print(f"Refactoring {file}")

            # Find the permission if it exists
            match = re.search(r'AuthorizeAsync\([^,]+,\s*(Permission\.\w+)', content)
            permission = match.group(1) if match else None

            # Remove auth block
            auth_block_pattern = r'\s*var\s+\w+\s*=\s*await\s+_branchAuthorizationService\.AuthorizeAsync[^\;]+;\s*if\s*\(\w+\.IsError\)\s*\{\s*return\s+\w+\.Errors;\s*\}'
            content = re.sub(auth_block_pattern, '', content)
            
            # Remove field
            content = re.sub(r'\s*private readonly IBranchAuthorizationService _branchAuthorizationService;', '', content)
            
            # Remove from constructor arguments
            content = re.sub(r',\s*IBranchAuthorizationService\s+[a-zA-Z0-9_]+', '', content)
            content = re.sub(r'IBranchAuthorizationService\s+[a-zA-Z0-9_]+\s*,', '', content)
            
            # Remove assignment
            content = re.sub(r'\s*_branchAuthorizationService\s*=\s*[a-zA-Z0-9_]+;', '', content)
            
            # Write handler back
            with open(handler_path, 'w', encoding='utf-8') as f:
                f.write(content)

            # Update the corresponding Command/Query if we found a permission
            base_name = file.replace("Handler.cs", "")
            cq_file = base_name + ".cs"
            cq_path = os.path.join(root, cq_file)
            
            if os.path.exists(cq_path):
                with open(cq_path, 'r', encoding='utf-8') as f:
                    cq_content = f.read()
                    
                modified_cq = False
                    
                # 1. Add Usings
                if "using PrimeFit.Application.Security;" not in cq_content:
                    cq_content = "using PrimeFit.Application.Security;\n" + cq_content
                    modified_cq = True
                if "using PrimeFit.Application.Security.Markers;" not in cq_content:
                    cq_content = "using PrimeFit.Application.Security.Markers;\n" + cq_content
                    modified_cq = True
                if "using PrimeFit.Domain.Common.Enums;" not in cq_content:
                    cq_content = "using PrimeFit.Domain.Common.Enums;\n" + cq_content
                    modified_cq = True
                    
                # 2. Add Attribute
                if permission and "[BranchAuthorize" not in cq_content:
                    cq_content = re.sub(r'(public\s+(?:sealed\s+)?(?:record|class)\s+' + base_name + ')', f'[BranchAuthorize(BranchPermissions = [{permission}])]\n    \\1', cq_content)
                    modified_cq = True
                    
                # 3. Add IBranchAuthorizedRequest
                if "IBranchAuthorizedRequest" not in cq_content:
                    if "IAuthorizedRequest" in cq_content:
                        cq_content = cq_content.replace("IAuthorizedRequest", "IBranchAuthorizedRequest")
                    else:
                        cq_content = re.sub(r'(IRequest(?:<[^>]+>)?)', r'\1, IBranchAuthorizedRequest', cq_content)
                    modified_cq = True
                        
                if modified_cq:
                    print(f"Updating {cq_file}")
                    with open(cq_path, 'w', encoding='utf-8') as f:
                        f.write(cq_content)
