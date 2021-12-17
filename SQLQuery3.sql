--SELECT Owner.*, Neighborhood.Name'NeighborhoodName', Dog.Id'DogId', Dog.Name'DogName', Dog.Breed, Dog.Notes, Dog.ImageUrl FROM Owner
--JOIN Neighborhood ON Owner.NeighborhoodId = Neighborhood.Id
--LEFT JOIN Dog ON Owner.Id = Dog.OwnerId
--WHERE Owner.Id = 1

SELECT Walks.*, Dog.Id'DogId', Owner.Name'OwnerName', Dog.OwnerId, Walker.Id'WalkerId', Walker.ImageUrl, Walker.Name'WalkerName', Walker.NeighborhoodId, Neighborhood.Name'nName' FROM Walks
JOIN Walker ON Walks.WalkerId = Walker.Id
JOIN Neighborhood ON Walker.NeighborhoodId = Neighborhood.Id
JOIN Dog ON Walks.DogId = Dog.Id
JOIN Owner ON Dog.OwnerId = Owner.Id
WHERE Walks.WalkerId = 3