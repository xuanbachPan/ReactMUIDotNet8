Create Table dbo.InputData
(InputID INT PRIMARY KEY IDENTITY (1, 1),
 RowNums INT, 
 ColNums INT,
 TreasureKeyNum INT,
 TableData VARCHAR(MAX) 
)

Create Table dbo.OutputData
(
 NoID INT PRIMARY KEY IDENTITY (1, 1),
 InputID INT,
 RowNums INT, 
 ColNums INT,
 PVal INT,
 MinFuelVal float,
 RouteID INT,
)

Create Table dbo.RouteData
(
 NoID INT PRIMARY KEY IDENTITY (1,1),
 InputID INT,
 RouteID INT,
 PositionNo INT, 
 XVal INT,
 YVal INT,
 KeyNumber INT
)