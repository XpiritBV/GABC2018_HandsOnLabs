# Machine Learning

Welcome to the Machine Learning workshop of the Global Azure Bootcamp. In this workshop, you will learn how to use Azure Machine Learning to predict who will be the next victim.

## Introduction

See the [presentation](GABC%202018%20Machine%20Learning.pdf) for some background information.

## About the data

Over the past years, we have compiled a data set about an infamous criminal 'family'. Members are born or hired into the family. The file to import can be found in [GitHub](https://raw.githubusercontent.com/XpiritBV/GABC2018_HandsOnLabs/master/ML/TheFamily.csv).

### The Family

The family consists of hundreds of members. There is a strict hierarchy, a rank determines how much power an individual member has.
Over the past years, many members have died because of violent interactions with other families and the law. We have noticed that women and children have a higher survival rate than men. Members of lower ranks have a lower survival rate than higher ranked individuals.

### Summary

For every family member, we compiled the following information:

- FamilyMemberId: Unique identifier
- Alive: Living or dead
- Rank:    
  1. Boss
  2. Leader
  3. Henchman
- Name: Name
- Gender: Gender
- Age:    Age or estimated age
- Parch: The number of parents and children the member has within the family    
- SibSp:    The number of siblings and spouses the member has within the family
- PassportNr: Passport number
- Income: Income in hundreds of $ per month
- LastSeenAtCode: Last seen in this segment of the map
- Neighborhood: 
  - S: Staten Island 
  - C: Chelsea  
  - Q: Queens

## Walkthrough

Either try it yourself using the file in [GitHub](https://raw.githubusercontent.com/XpiritBV/GABC2018_HandsOnLabs/master/ML/TheFamily.csv) or use the below steps to complete the workshop. In [step 3](step3.md) you will find the hints needed to complete the exercise.

1. [Step 1](step1.md); build the ML model
2. [Step 2](step2.md); deploy web service
4. [Step 3](step3.md); find the victim!
