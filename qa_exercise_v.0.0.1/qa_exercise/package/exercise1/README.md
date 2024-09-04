<!--v001-->
# Stargate

***

## Astronaut Career Tracking System (ACTS)

ACTS is used as a tool to maintain a record of all the People that have served as Astronauts. When serving as an Astronaut, your *Job* (Duty) is tracked by your Rank, Title and the Start and End Dates of the Duty.

This interface is to provide a Person a place to update their Astronaut Duty History. They can add or remove any duty that was assigned to them.

### Requirements

##### Implement test cases: (Required)

The UI is expected to do the following:

1. Sign in a person. If there is an incorrect username or password, the system will alert the user.
1. An authenticated user can view their duty history.
1. An authenticated user can delete any of their duties.
1. An authenticated user can add a new duty. All fields are required. The end date can't come before the start date. The start and end date can't overlap any existing duty.

##### Implement API tool: (Required)

The API is expected to do the following:

1. Sign in a person. The session is stored in a cookie named 'ACTS_Session'.
1. Sign out a person.
1. An authenticated user can get their Person information. It should return an Id and a Name.
1. An authenticated user can get their Astronaut Duty history. It should be ordered by descending Duty Start Date.
1. An authenticated user can delete any of their duties.
1. An authenticated user can add a new duty. The start and end date can't overlap any existing duty.

### Tasks

Overview

1. Run the application (home.html)
   * Username: john.doe
   * Password: abc123
1. Implement automated tests
   * Cypress (preferred)
   * Selenium
   * Playwright
1. Implement an API tool
   * Postman (preferred)