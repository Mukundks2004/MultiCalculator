# MultiCalculator

## Guide to TaskMate (previously known as MultiCalculator)
How to use:
1) Clone repository
2) Run dotnet ef database drop
3) Run dotnet ef database update
4) Uncomment "Seed Data" code in LoginWindow.xaml.cs (you can recomment this after first run to stop clearing db)
5) Run! have fun!

## Features:

### Login:
- Enter details to login
- If you enter incorrect details you cannot log in, the system gives you feedback

### Scientific Calculator
- As you type dynamically checks whether expression is formatted correctly, and if it is presents it in latex
- Creates abstract syntax tree and evaluates it
- Can add packages if created and copy output

### Chatbot
- Can talk to chat GPT via prompt
- Records your past conversations, so you can go back and continue them

### Plugin Creator
- Can upload a dll with operations in it (that implement IToken)
- Can change x and y and size, and save a package consisting of many buttons

### Settings
- Can change username
- Can change theme

### History
- Can see recently submit questions
- Can select them by clicking and turn them into a pdf homework style (questions and answers)

### Generate practice questions
- Can generate questions and answers for testing
- Can send them to a user's email in case they are using this remotely

---

## WHAT EVERYONE IS WORKING ON
- Mukund:
  - README
- Dennis: Setup OPENAI, has $10 credit, we still need testing. AI/Database basically has been setup.

## TODO
Mukund:

- Graphing calculator (estimated time: 3 hrs)
- General cleaning (estimated time: 90 min)
  - Parser outputs nullary lambda rather than double
  - 
- Make all windows look ok when resized or non resizeable otherwise
- Hope the project can be net8.0-windows because it is
