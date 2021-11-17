CREATE VIEW dbo.SamuraiBattleStats
AS
SELECT dbo.Samurais.Name,
COUNT(dbo.SamuraiBattle.BattleId) AS NumberOfBattles,
		dbo.EarliestBattleFoughtBySamurai(MIN(dbo.Samurais.Id)) AS EarliestBattle
FROM dbo.SamuraiBattle INNER JOIN
	 dbo.Samurais ON dbo.SamuraiBattle.SamuraiId = dbo.Samurais.Id
GROUP BY dbo.Samurais.Name, dbo.SamuraiBattle.SamuraiId