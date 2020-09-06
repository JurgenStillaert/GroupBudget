# GroupBudget
This project will be my reference project with implementations of DDD, Clean Architecture (Hexagonal Architecture / ports and adapters), CQRS, Event Sourcing and many other things I believe are best practices concerning software development.

I will use the Microsoft stack (.NET, C#, Azure) and Angular 2+.

This project will be build gradually, and maintained progressively when I gain new insights. 

## Business problem

My partner and I do not have a common bank account when we spent money on groceries and other costs related to living together. At the end of the month we calculate everything we've paid with our personal accounts, and determine who has to pay what amount to the other person.

But in reality, at the end of the month we're looking at our bank accounts, try to figure out what we've paid for what reason, write it down and calculate the sums manually. It takes quite a lot of time and it's an administrative task we could easily automate.

## Functional analysis

We could do an event storming session on this domain, but as it is fairly easy business problem, we'll take a top down approach, starting from wireframes.

### Login

![Login page](https://github.com/JurgenStillaert/GroupBudget/blob/master/Documentation/login_page.png?raw=true)



### Register user

![Register user](https://github.com/JurgenStillaert/GroupBudget/blob/master/Documentation/register_user.png?raw=true)

### Forgot password

| ![Pasword forgotten](https://github.com/JurgenStillaert/GroupBudget/blob/master/Documentation/password_forgotten.png?raw=true) | ![New password](https://github.com/JurgenStillaert/GroupBudget/blob/master/Documentation/new_password.png?raw=true) |
| :----------------------------------------------------------: | :----------------------------------------------------------: |
|                                                              |                                                              |

### Month overview

In the month overview (or cycle overview - as you can change the duration of a cycle in the settings), you can view, add, edit and delete the bookings you've made for that period. You can also switch to another month. You can close the month. 

![Cycle overview](https://github.com/JurgenStillaert/GroupBudget/blob/master/Documentation/cycle_overview.png?raw=true)

### Closed month

You can review a closed month. If the other person(s) in your group also closed their month, a message of how much you have to pay to the other person or how much you have to receive is displayed.

![cycle_closed.png](https://github.com/JurgenStillaert/GroupBudget/blob/master/Documentation/cycle_closed.png?raw=true)

### Settings

![settings.png](https://github.com/JurgenStillaert/GroupBudget/blob/master/Documentation/settings.png?raw=true)

### Tool used to create wireframes

I've used this great tool to make wireframes: https://pencil.evolus.vn/