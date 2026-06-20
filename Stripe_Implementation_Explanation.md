# شرح تطبيق Stripe في المشروع (Stripe Implementation)

أهلاً بك! أولاً، **ملاحظتك ممتازة جداً**. في البداية قمت بإنشاء جدول `SubscriptionPlan` منفصل كإجراء احترازي لفصل خطط الدفع عن الـ `Package` الخاص بالجيم، تجنباً للتدخل في الـ Business Logic الحالي. ولكن بما أن الـ `Package` بالفعل يحتوي على السعر والمدة (`Price` و `DurationInMonths`)، فاستخدامه هو **الخيار الأفضل والأكثر نظافة**.

لقد قمت بتعديل الكود فوراً بناءً على ملاحظتك:
1. تم حذف `SubscriptionPlan` بالكامل.
2. تم إضافة `Currency` لجدول الـ `Package`.
3. تم ربط `PaymentTransaction` بالـ `Package` بدلاً من الخطة المنفصلة.

---

## تفاصيل الـ Implementation للـ Stripe عبر الطبقات (Layers)

تم تطبيق الـ PaymentIntents و الـ Webhooks بشكل يتوافق تماماً مع الـ Clean Architecture والـ CQRS في مشروعك:

### 1. طبقة الـ Domain (الكيانات - Entities)
* **تعديل `Package.cs`**: تم إضافة `Currency` لتحديد العملة لكل باقة وتحديد الافتراضي لها بـ "egp".
* **إنشاء `PaymentTransaction.cs`**: كيان جديد لتتبع دورة حياة عملية الدفع. يحتوي على (رقم المستخدم، رقم الباقة، `StripePaymentIntentId`، المبلغ، والعملة).
* **إنشاء `PaymentTransactionStatus.cs`**: Enum يحدد حالة الدفع (`Pending` قيد الانتظار، `Succeeded` ناجح، `Failed` فاشل).
* تم تحديث `ErrorCodes.cs` وإضافة أكواد خطأ خاصة بعمليات الدفع مثل `TransactionNotFound`.

### 2. طبقة الـ Application (الـ Use Cases والـ Interfaces)
* **إنشاء `IStripePaymentService`**: واجهة (Interface) لتعريف العمليات التي نحتاجها من Stripe (إنشاء PaymentIntent، والتحقق من الـ Webhook) بدون ربط طبقة الـ Application بمكتبة Stripe مباشرة.
* **إنشاء `InitializePaymentCommand`**: أمر (Command) يتم إرساله عند ضغط المستخدم على "دفع". يقوم بالتحقق من وجود الباقة المطلوبة، ويتصل بـ Stripe لإنشاء `PaymentIntent`، ثم يحفظ `PaymentTransaction` بحالة `Pending` في قاعدة البيانات. يُرجع الـ `ClientSecret` للـ Frontend لإكمال الدفع.
* **إنشاء `FulfillPaymentCommand`**: أمر (Command) يعمل **فقط** عند وصول إشعار (Webhook) من Stripe يفيد بنجاح الدفع.
  * **ميزة مهمة (Idempotency)**: هذا الكوماند يرث من `IdempotentCommand` ويستخدم الـ Event ID الخاص بـ Stripe كـ `RequestId`. هذا يضمن أنه حتى لو قام Stripe بإرسال نفس الـ Webhook مرتين، فلن يتم تفعيل الاشتراك مرتين.
  * في هذا الكوماند، يتم البحث عن الـ `PaymentTransaction` المعلق، تحويل حالته إلى `Succeeded`، ومن ثم تفعيل الاشتراك للمستخدم عبر استدعاء `CreateSubscriptionAsync`.

### 3. طبقة الـ Infrastructure (التنفيذ وقاعدة البيانات)
* **تثبيت مكتبة `Stripe.net`**: مكتبة Stripe الرسمية.
* **إنشاء `StripeOptions.cs` و `StripePaymentService.cs`**: لتنفيذ `IStripePaymentService`. يتم قراءة إعدادات Stripe (المفاتيح السرية) باستخدام نمط `IOptions`.
* **الـ Database Configurations**:
  * إضافة `PaymentTransactionConfiguration` بـ Schema منفصل `"payments"` مع عمل `Unique Index` على `StripePaymentIntentId`.
  * إضافة `Currency` إلى `PackageConfiguration`.
* **تحديث `AppDbContext` و `UnitOfWork`**: لإضافة الـ Repositories الجديدة والـ DbSets الخاصة بالـ `PaymentTransaction`.

### 4. طبقة الـ API (الـ Controllers)
* **إنشاء `PaymentsController`**: يحتوي على مسارين (Endpoints):
  1. `POST /api/payments/initialize`: (يتطلب تسجيل دخول) يستقبل `PackageId` ويعيد الـ `ClientSecret`.
  2. `POST /api/payments/webhook`: (مفتوح بدون تسجيل دخول، وبدون Rate Limiting) لأن خوادم Stripe هي من تقوم بالنداء عليه. يقوم هذا المسار بـ:
     * قراءة الـ Raw Body للـ Request.
     * التحقق من التوقيع الرقمي (Signature) الخاص بـ Stripe للتأكد من أن الطلب لم يتم التلاعب به.
     * إذا كان الحدث هو `payment_intent.succeeded`، يتم تحويل الـ Event ID إلى `Guid` ثابت وإرسال `FulfillPaymentCommand` لتفعيل الاشتراك.

---

### الخطوات القادمة لك:
1. **قاعدة البيانات**: عمل Migration جديد وإرساله لقاعدة البيانات:
   ```bash
   dotnet ef migrations add AddPaymentEntities --project src/PrimeFit.Infrastructure --startup-project src/PrimeFit.API
   ```
2. **إعدادات Stripe**: تعبئة القيم في ملف `appsettings.Development.json` أو ملف الـ `.env`:
   * `STRIPE_PUBLISHABLE_KEY`
   * `STRIPE_SECRET_KEY`
   * `STRIPE_WEBHOOK_SECRET`
3. **تحديث العملات**: ستحتاج إلى تحديد العملة المناسبة لكل باقة في قاعدة البيانات في حقل `Currency` (القيمة الافتراضية هي "egp").
4. **اختبار الـ Webhook محلياً**: يمكنك استخدام أداة Stripe CLI أثناء التطوير لاختبار الدفع:
   ```bash
   stripe listen --forward-to https://localhost:44000/api/payments/webhook
   ```
