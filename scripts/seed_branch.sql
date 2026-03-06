BEGIN TRANSACTION;

BEGIN TRY

DECLARE @OwnerId INT = 1;
DECLARE @Now DATETIMEOFFSET = SYSDATETIMEOFFSET();

DECLARE @Gov TABLE
(
    Id INT IDENTITY,
    GovernorateId INT,
    Latitude FLOAT,
    Longitude FLOAT
);

INSERT INTO @Gov (GovernorateId, Latitude, Longitude)
VALUES
(1, 30.0444, 31.2357),
(2, 31.2001, 29.9187),
(3, 30.0333, 31.2333),
(4, 30.5965, 32.2715),
(5, 31.0364, 31.3807),
(6, 30.7865, 31.0004),
(7, 31.4175, 31.8144),
(8, 31.1090, 30.9361),
(9, 30.5877, 31.5020),
(10,30.9410, 31.3785),
(11,29.9668, 32.5498),
(12,27.1800, 31.1837),
(13,26.5591, 31.6957),
(14,24.0889, 32.8998),
(15,25.6872, 32.6396);

;WITH Numbers AS
(
    SELECT TOP (500000)
        ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS n
    FROM sys.objects a
    CROSS JOIN sys.objects b
    CROSS JOIN sys.objects c
)

INSERT INTO Branches
(
    Name,
    Email,
    PhoneNumber,
    BranchType,
    BranchStatus,
    GovernorateId,
    Address,
    Coordinates,
    OwnerId,
    IsDeleted,
    CreatedAt,
    CreatedBy
)
SELECT
    CONCAT('PrimeFit Branch ', n),
    CONCAT('branch', n, '@primefit.com'),
    CONCAT('+2010', RIGHT('0000000' + CAST(n AS VARCHAR),7)),
    'Mixed',
    'Active',

    g.GovernorateId,

    CONCAT('Street ', n, ', Egypt'),

    geography::Point(
        g.Latitude +
        (SQRT(RAND(CHECKSUM(NEWID()))) * 0.15)
        * COS(2 * PI() * RAND(CHECKSUM(NEWID()))),

        g.Longitude +
        (SQRT(RAND(CHECKSUM(NEWID()))) * 0.15)
        * SIN(2 * PI() * RAND(CHECKSUM(NEWID()))),

        4326
    ),

    @OwnerId,
    0,
    @Now,
    @OwnerId

FROM Numbers
CROSS APPLY
(
    SELECT TOP 1 *
    FROM @Gov
    ORDER BY NEWID()
) g;

COMMIT TRANSACTION;

PRINT '500000 branches inserted';

END TRY
BEGIN CATCH

ROLLBACK TRANSACTION;
PRINT ERROR_MESSAGE();

END CATCH;