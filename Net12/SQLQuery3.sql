SELECT Us.*
FROM 
(SELECT U.*
FROM Users U
LEFT JOIN  
(SELECT Id, MIN (Name) Name
FROM
(SELECT TempU.Name,TempU.Id
FROM
(SELECT U.Name,U.Id
FROM Users U) TempU
LEFT JOIN Users U ON TempU.ID!=U.Id
WHERE TempU.Name=U.Name) TempAll
WHERE TempAll.Id NOT IN
(SELECT RepeatUser.Id
FROM 
(SELECT TempU.Name, MIN(TempU.Id) Id
FROM
(SELECT U.Name,U.Id
FROM Users U) TempU
LEFT JOIN Users U ON TempU.ID!=U.Id
WHERE TempU.Name=U.Name
GROUP BY TempU.Name) RepeatUser )
GROUP BY Id) R
ON U.Id=R.Id
WHERE R.Id=U.Id) Us
WHERE Us.IsActive=1