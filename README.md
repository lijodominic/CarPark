# CarPark

API that does a rate calculation engine for a carpark
The inputs for this engine are:
1. Car Entry Date and Time
2. Car Exit Date and Time

Based on these 2 inputs the engine program should calculate the correct rate for the customer and
display the name of the rate along with the total price to the customer based on rate configured

Swagger is added to the application for API documentation.

The api takes in 2 date inputs as string in mm/dd/yyyy HH:mm

sample input

{
  "entryDate": "09/16/2021 07:50 PM",
  "exitDate": "09/16/2021 11:50 PM"
}


or

{
  "entryDate": "09/16/2021 19:50",
  "exitDate": "09/16/2021 23:50"
}

or

{
  "entryDate": "2021-09-16T19:50:06",
  "exitDate": "2021-09-16T23:50:06"
}
