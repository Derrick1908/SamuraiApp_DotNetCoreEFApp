CREATE FUNCTION [dbo].[EarliestBattleFoughtBySamurai](@samuraiId int)
	RETURNS CHAR(30) AS
	BEGIN
		DECLARE @ret CHAR(30)
		SELECT TOP 1 @ret = Name
		FROM Battles
		WHERE Battles.Id IN (SELECT BattleId
							 FROM SamuraiBattle
							 WHERE SamuraiId = @samuraiId)
		ORDER BY StartDate
		RETURN @ret
	END