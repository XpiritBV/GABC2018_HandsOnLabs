# Step 3; find the victim!

In the previous step, we created the web service. Now you have a way to talk to your model and get predictions back.

The web service will look like something below:

![azuremlwebservice.png](azuremlwebservice.png)

There is both a request/response endpoint as well as a batch endpoint and both contain instructions on how to consume. The preview pages of the Test also contain an online test form and even ready to use C# code to call the API.

## Who will be killed next?

We built this model to predict who will be the next victim and with some hints, we should be able to query the system and find out who will most likely not survive the night.

Use the following data and record the outcome.

|Rank|Gender|Age|SiblingSpouse|ParentChild|Income|Neighbourhood|
|--|--|--|--|--|--|--|
|3|female|34.5|0|0|7.8292|Q|   
|3|female|18|0|0|7.2292|C|     
|3|male|17|0|0|7.8958|S|      
|1|male|40|1|5|311.3875|C|    
|1|male|40|1|5|19.3875|Q|

**Note:** you can use the button 'Test' to feed the parameters to the service for easy testing. 
