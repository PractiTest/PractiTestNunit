Feature: Login
Perform login operation in EA Site

    @smoke
    Scenario: Perform Login operation in EA Site
        Given I navigate to the application
        And I click Login
        And I login with following details
          | UserName | Password |
          | admin    | password |
        Then I should see the "Employee Detail"