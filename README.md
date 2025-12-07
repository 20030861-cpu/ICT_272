# ICT_272

# Codebase Improvements Overview

## Change 1: Proper Object-Oriented Design
The original program placed almost all logic inside the `Main` method, and even had a `Program` class constructor that was never used. This went against object-oriented principles and made the code difficult to maintain.  
The refactored version introduces a proper structure with dedicated classes:

- **Player** – holds player information through a clear constructor.  
- **RegistrationManager** – handles business logic and cost calculations.  
- **DisplayManager** – responsible only for formatting and outputting results.  
- **InputValidator** – manages all user input and validation.

This restructuring follows the Single Responsibility Principle, keeping each class focused on one purpose, resulting in cleaner, more maintainable code.

## Change 2: Eliminate Code Duplication
The original code duplicated price calculation logic across several `if-else` blocks, making updates difficult and error-prone.  
The refactored version replaces all repeated blocks with a single reusable method:

`CalculatePlayerCost(registrationType, jerseyOption, groupDiscountEligible)`

Pricing values were also extracted into constants:
- `KIDS_BASE_COST = 150`
- `ADULT_BASE_COST = 230`
- `JERSEY_COST = 100`

This reduced roughly forty lines of repetitive logic down to about ten, and any future pricing updates now require changes in only one place.

## Change 3: Fix Summary Display Logic
Originally, the summary table headers were printed inside the loop that collected player input, leading to multiple duplicated headers and messy formatting when more than one player was added.

The improved design separates data entry from output.  
All players are first stored in a list, and after collection is complete, the summary is displayed once by the dedicated `DisplaySummary` method.  
This method prints clean column headers, separators, individual player details, and total counts — resulting in a professional, easy-to-read summary.

## Change 4: Robust Input Validation
The initial program relied on `Convert.ToInt32`, which crashes the application on invalid input and provided no retry mechanism.  
The new `InputValidator` class implements robust, user-friendly validation using `int.TryParse` and retry loops until proper input is entered.

It validates:
- Number of players  
- Registration type (Kids/Adult only)  
- Jersey choice (flexible yes/no input)  
- Player names (minimum length, no whitespace issues)

Clear error messages help the user correct mistakes without crashing the program, making the overall experience far more reliable and smooth.
