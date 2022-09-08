# Dikke Tennis Lijst



[TOC]

### Names

1. fifteenfourty.com



### Backend

Nothing here yet.



### Frontend

Use Angular Universal to render the first page on server. Then have the first page static rendererd version have a wheel that turns until the app itself is loaded. This is better for SEO and non-JavaScript devices.



Do a server cache, and a private client cache. Do some kind of invalidation of cache on any controllers CUD-methods completion and then some kind of validation of cache based on some GUID. Speeds the website up by a lot. Get a guide



##### Authorization

Check session storage, cookie storage, refresh tokens;



##### Styling

Notifier should sometimes just pop up to tell you stuff.

Like a nice little message box that is talking with you and has personality, also commenting on things you do on the website.

Put some eyes on the screen that track the mouse at some point;



Logo is a crown with tennis balls.
Logo is placed above the first in the list, he is the king.
Logo when scrolling down
	will become smaller
	will skew vertically
	will become grey
	until it fits nicely in the top bar, that stays.
	or place the actual logo in the middle of the crown, and that will be the logo of the scrollbar.
		- logo can be db
		- logo can be dikketennislijst
		- logo can be (3) arrows up, same as the arrow up you get when you go up in the list.
Logo onClick scroll to top.


Menu SubBar soft blue for messages


Every game counts.


design: no header. Only an icon or just first name you can click on for profile stuff.
	bigg header is also a thing.

front (all) page : list of players based on ranking.

click on a rating block (...player, elo) you see some details
click on a player you go the players page
	Players page
		> player can do shit here, like manage his bio, manage his photo.
		> stats are shown, nice graphs and shit;
		> comments can be made maybe.


click on a clubc you go to the clubs page (not necessary, can be done later, can just be a filter either)
