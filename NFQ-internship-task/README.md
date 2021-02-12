# How to use
- ### User
To book an appointment go to https://nfq-task.azurewebsites.net/appointment and click "Book new appointment". You will be presented with a reservation code. You can use your reservation code to check how much time is left until the appointment starts at https://nfq-task.azurewebsites.net/check or cancel the appointment at https://nfq-task.azurewebsites.net/cancel.

- ### Specialist
To view the website as a specialist, first you need to login https://nfq-task.azurewebsites.net/login
Specialist1 login: `Specialistas1`
password: `SpecialistasVienas1.`

Specialist2 login: `Specialistas2`
password: `SpecialistasDu1.`

Specialist3 login: `Specialistas1`
password: `SpecialistasTrys1.`

After loggin in you can see appointments registered to you at https://nfq-task.azurewebsites.net/appointment. To start, end or cancel an appointment click the respective button next to the appointment in the table.

To view the display board go to https://nfq-task.azurewebsites.net/displayboard.

# Display board requirements
- ##### The service department screen should show current visits and five upcoming visits.  
DisplayBoardView component visualizes appointments received from the backend. Backend receives authorized api call and provides all appointments with status Active along with 5 appointments with earliest time and status Waiting.

- ##### The service department screen information must be updated every five seconds.
A basic timer with period of 5 seconds is initiated on loading the display board page.

- ##### The service department screen must not be publicly accessible.
Display board page and api is protected with authentication.  

# Visit reservation requirements
- ##### The customer must be able to reserve an appointment with a specialist (the customer does not need to register an account in the system). The user does not select any specific time, the system calculates the next available appointment time. After a successful reservation, the system must generate a reservation code and provide it to the customer.
Appointment page (when not authenticated) shows a button to book an appointment. When clicked, a request to the api is sent and the backend finds the soonest available specialist (replaces canceled appointment or fills in an empty timeframe, or places the appointment after another booked appointment whichever is the earliest option). After sucessfull reservation, the appointment reservation code is generated and the appointment data is sent back to the client. Specialists have specific working hours implemented and the booking logic respects that.

- ##### The customer must see how much time is left before the visit (separate page, not the service department screen).
Check appointment page with reservation code input and button to send the request to backend. Backend looks up the appointment by reservation code and sends back a TimeSpan, if appointment not found it sends a TimeSpan with -1 second, which the client-side handles as "wrong reservation code".

- ##### The customer must be able to cancel the visit.
Cancel appointment page with very simillar frontend as Check appointment page, except the backend either marks the appointment as canceled and the user is redirected to homepage or sends back an empty appointment, which in client-side is interpreted as "wrong reservation code".

# Customer visit management requirements
- ##### The specialist must have an account (can be created through a database, no administration is required for accounts) with which to log in to visit management.
Accounts are created on startup with database seeding.

- ##### The specialist should only see patients who have registered with him.
Appointment page when authenticated shows appointments registered to the authenticated user. Upon entering the page, a request to the api is sent, backend checks user identity and queries the database accordingly with the right specialist id for appointments.

- ##### The specialist must be able to mark that the visit has begun. There can only be one active visit at a time.
In the appointment page specialist is presented with start button on each appointment, when clicked it sends a request to the backend to try and mark it as Active, if there are no active appointments it marks it as Active and sends back a response "true", otherwise "false".

- ##### The specialist must be able to mark the end of the visit.
Same logic as starting an appointment.

- ##### The specialist must be able to cancel the visit.
Same logic as starting an appointment.
