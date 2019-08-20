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

