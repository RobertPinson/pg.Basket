# Pure Gym Basket Test

The solution consists of four projects  

* Business logic layer   
     * Contains Basket service to implementing application flow logic and expose the domain model to a UI.
* Data access layer
   * Contains Entity Framework Core DB context and repository for persisting to a SQL DB.
* Domain Models
   * Contains domain models encapsulating data model and logic in domain entities. 
* Unit Tests
   * Five unit tests covering the required scenarios.

TODO
* Complete data access layer.
* Add Integration tests.
* More unit tests to increase coverage.

### Summary
I wanted to use the test as a learning exercise for me to follow DDD Principles and Clean Architecture [here](https://www.thereformedprogrammer.net/three-approaches-to-domain-driven-design-with-entity-framework-core/), which is why the logic is in the domain models (DDD styled entities) and not in Business Logic Layer. 

The project builds and the unit tests pass, i hope it is enough to demonstrate the skills you are looking for at Pure Gym.  