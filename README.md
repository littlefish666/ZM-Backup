# ZM-Backup
Zmodeler Backup Program

ZM3 backup by LF666

some stuff you should know:

THE SaveInterval IS ACTUALLY TRANSFER INTERVAL

never worry about messed up models or overwritten autosaves again!
if you've got Zmodeler open before, make sure to close it otherwise it'll get force closed
reason the program must launch ZMod is so timings are perfect
the minumum save time is 1 minute due to folder name (created by minutes), if you try putting less , e.g. a decimal , it'll still interval at 1 minute
the reason the log contains so much lines about searching for Zmodeler3 process it because it scans through all processes and searches for Zmod one, even after it's found it
the save interval MUST be in minutes, this is the converted to milliseconds internally, and then a few seconds is added on to compensate for the actualy time it takes to save and compress the z3d
if you put the interval in milliseconds, it won't ever save
if you press any keys whilst program is running , it'll stop, - (i can't completely block keyboard input with this) , I've put in a buffer for 5, but after that it just closes
the reset section on the ini is whether you want to reset the ini on program run, if you've messed up the INI, just set it to true, run the program and it'll automatically correct itself
after correcting itself it will close and then you can amend the settings and just re run the program
Source will be available shortly after final version
side note: this doesn't hook into Zmodeler or anything, no hooks, m3m0ry hax or anything involved, just changing the profile.xml
if there's any things not working or your profile.xml isn't compatible with this, please let me know, either through the issues on Github or through the mail - 3530441844@qq.com

how it works

takes value from ini
changes profile.xml to house that value
starts Zmodeler with this modified profile.xml
Zmodeler now saves in those intervals, and plugin transfers them just after the save has occured into folder names by timestamp 
if you're noticing it's trying to transfer whilst it's still saving, turn compression off (z3d takes up more space) , turn embed textures off, or increase the delay , all of which can be done from INI or Zmod

if you've messed up somwhere :

to import an autosave.z3d > go to either merge or import, if "open" doesn't work
the file it's moving will always be called autosave.z3d, if not it won't work
if you've changed the value in Zmodeler whilst the plugin is running, it'll just carry on copying at these intervals, 
e.g. if you've increased interval , you'll find at least 2 folders with same z3d, if you've deceased interval, you'll find that it won't copy over enough times
reason I added seconds is so it doesn't try to create folder with same name if interval it less then 60s


things for the future
maybe a gui, don't really see the point in it though
differentiating between different files open in Zmodeler so that if you open & close different files regularly, you'll be able to sort by them
ZM API "cough cough"


-lf666


edit: i pasted from notepad and formatting didn't carry across , so it doesn't all make entire sense but whatever : p
