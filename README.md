# Library
<h3>BBLibrary is a test application for a Big Band.</h3>
<br>
<div>
It has:
<ul>
<li>List of all library music charts for a Big Band and Dixie Band.</li> 
<li>List of Artists, who are current members of the Band and available artists, who can be invited for a performance.</li>
<li>List of Instruments, artists can play and which are used in arrangements of the charts.</li>
<li>List of Venues where the Band had or is going to have concerts.</li>
<li>List of future or past Performances.</li>
</ul>
</div>
It was written in Visual Studio 2017 using ASP.NET Core 2.2.
<div>
<h3>There are three access levels:</h3>
<ol>
<li>Authorized person can browse through all library music charts. (No CRUD operations)</li>
<li>Member can browse through all library music charts and look at the future and past performances. Can explore the concert program. Can see who played or is going to play and which part (or instrument) in an arrangement of the chart during the performance.
If the arrangement of the piece and the artists list has not been set by the AppAdmin, member's access to this page will be denied.  (No CRUD operations)</li>
<li>AppAdmin has unrestricted access to all resources. (CRUD operations: Create, Read, Update, Delete).</li>
</ol>
</div>
In this application Identity was scaffolded and modified in a way that it is now required a confirmed email before login. So, if somebody wants to browse through the charts, they need to register first, then to login in their email account and click the link to confirm their email address.
<p>
Only then they can login to the BBLibrary.
</p>
To use EmailSender service edit your email server settings in <code>appsettings.json</code>
<p>If you are going to use Gmail you need to turn on Less Secure Apps permission in a Gmail account, otherwise all messages about confirmation of an email address will be blocked.
</p>
<h3>To run this application:</h3>
<ul>
  <li>Update your server settings in connection strings in the <code>appsettings.json</code> file. 
If you are using Windows authentication and MSSQLLocalDB it is:<code>“Server=(localdb)\\mssqllocaldb”.</code> Nothing needs to be changed.
If you are using MSSQL Server and using Server authentication, then it is:
<p><code>“Server=name_of_the_Server; Database=BBLibraryApp;MultipleActiveResultSets=true User Id=your_username;password=your_password”</code></p>
  <p>Make sure that the connection string written in one unbroken line.</p></li>
  <strong>Then run these commands in the root: </strong>(in the folder where there is <code>BBLibraryApp.csproj</code> file)
  <li><code>dotnet restore</code> (to restore all dependencies)</li>
  <li><code>dotnet build</code></li>
  <li><code>dotnet ef database update -c BBLibraryApp.Data.ApplicationDbContext</code> (to create identity database)</li>
  <li><code>dotnet run</code> (to create library database, seed data to it, seed AppAdmin to the identity database and adding Administrator claim type to the AppAdmin user)</li>
  <strong>Now you can go to: <code>https://localhost:44320/</code></strong>
  
  <li>Login as <code>AppAdmin</code>,</li>
  <li>Password: <code>P@ssw0rd</code></li>
  </ul>
  <p>
  In this application as an example a claims-based identity and a roles-based identity were used. In the <code>Startup.cs  services.AddAuthorization</code> with <code>AddPolicy</code> option was added. So, now we have two authorization policies: <code>AdministratorOnly</code> and <code>MembersOnly</code>.
  To satisfy the requirement for the <code>AdministratorOnly</code> policy you need to have an <code>AdministratorClaimType</code>, which is in our case a unique string:
  <p><code>const string AdministratorClaimType = "http://BBLibraryApp.com/claims/administrator";</code></p>
  <p>To satisfy the requirement for the <code>MembersOnly</code> policy you have to be in a role of <code>Member</code>.
  This policy applies to <code>Index</code> and <code>Details</code> actions in the <code>Performances Controller</code>:

<code>[Authorize(Policy ="MembersOnly")]</code>
 
As it has been mentioned before we seeded AppAdmin to the identity database and added Administrator claim type to it. So now, for AppAdmin to have access not only to the resources with the authorization policy for <code>AdministratorsOnly</code> but also to the resources with the authorization policy for <code>MemberOnly</code>, we need to add AppAdmin to the <code>Member</code> role.
In this application AppAdmin has the ability to manage users and roles.</p>
<p>Go to the <code>Admin>Roles</code> and create role - <code>Member</code>.</p>
<p>Then go to <code>Admin>Users</code> and Edit AppAdmin user. Add him/her to <code>Member</code> role. Save changes.</p>
<p>Now we can go to Charts list and add 4 charts (1,3,7,8) to the briefcase, which we are going to play in the next concert.
You can see that every time we are adding chart to the briefcase there is the information about how many items we have and the total time. We use a session cart for this feature.</p>
<p>Before you can go to the <code>Performances</code> and create one, you need to implement changes in your identity status. You have to logout and login again. Otherwise access will be denied.</p>
<p>Because we have 4 charts in a briefcase, we can Click on the <code>Add Charts</code> button and attach these charts to the performance.</p>
<p>Now we can Edit performance and add Chart List order for the Concert.</p>
<p>After completing the program list, we can convert it to <code>pdf</code> format by clicking <code>Print</code> button.</p>
<p>Now we navigate back to the details of the performance.</p>
<p>By clicking the chart <code>Title</code> we can explore the instrumentation of the chart. If it was not set (it was not, as we seeded all charts without instrumentation) then we are redirected to the Edit section of the chart, where we can add all instruments used in the arrangement. <code>Check All</code> checkbox added all default instruments for the Big Band. Save the changes.</p>
<p>So now, if we go back to the performance and click to the title of the chart, we can see instrumentation and on the right-side artists list. At the moment it has not been set yet. Go to the <code>Edit Artists List</code> and set it. Save the changes.</p>
<p>Now we can keep track of all concerts we played, what was the program and who played it.</p>

