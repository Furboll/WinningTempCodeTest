# WinningTempCodeTest
Below are the instructions given by WinningTemp for the code test.

## Description
Attached is a program for calculating toll fees for vehicles in Gothenburg. There is room for several improvements to the program.  
The task is to refactor the code so that you can present something you stand behind and are satisfied with.
NOTE: The original files have been removed as they have been redone entirely.

## Requirements
Each passage through a toll station in Gothenburg costs **8**, **13**, or **18 SEK** depending on the time of day.  
The **maximum charge per day and vehicle** is **60 SEK**.

### Toll Fee Schedule

| Time Interval       | Amount |
|---------------------|--------|
| 06:00 – 06:29       | 8 SEK  |
| 06:30 – 06:59       | 13 SEK |
| 07:00 – 07:59       | 18 SEK |
| 08:00 – 08:29       | 13 SEK |
| 08:30 – 14:59       | 8 SEK  |
| 15:00 – 15:29       | 13 SEK |
| 15:30 – 16:59       | 18 SEK |
| 17:00 – 17:59       | 13 SEK |
| 18:00 – 18:29       | 8 SEK  |
| 18:30 – 05:59       | 0 SEK  |

---

### Additional Rules

- A congestion tax is charged for vehicles passing a toll station **Monday to Friday between 06:00 and 18:29**.
- No tax is charged on:
  - Saturdays
  - Public holidays
  - Days before a public holiday
  - During the entire month of **July**
- Certain vehicle types are **exempt** from the congestion tax.
- A car passing **multiple toll stations within 60 minutes** is only taxed **once**.  
  The amount charged will be the **highest fee** among those passages.


## Instructions to run the program
- Download the solution.
- Open the solution.
- Build the solution.
- Run the tests.
