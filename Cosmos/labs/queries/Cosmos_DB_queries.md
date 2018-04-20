Query for the murder offense id on the 29th Jan 2014:

```
SELECT * FROM complaints
WHERE complaints.KY_CD = "101"
AND
complaints.CMPLNT_FR_DT = "2014-01-29T00:00:00"
```

Query for taxi pickup locations within 600 meters of the coordinates which correspond with the murder with a pickup time of a few minutes around the murder date & time:

```
SELECT * FROM taxitrips
WHERE taxitrips.pickup_datetime > "2014-01-29T00:05:00"
AND 
taxitrips.pickup_datetime < "2014-01-29T00:10:00"
AND
ST_DISTANCE(taxitrips.pickup_location, {'type': 'Point', 'coordinates':[-73.882309442, 40.764803027]}) < 600
```