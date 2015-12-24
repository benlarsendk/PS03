# PS03

PS03 is a project which goal is to set more focus on security. Stop saving your passwords in chrome, firefox, don't automaticly connect to WiFi's and so on. This project shows how easy an attacker can get all your passwords ever saved, even without any anti-virus complaining. 

PS03, Password Sniffer, version 3, consists of four solutions:

  - PS03 Server
  - PS03 Victim
  - ChromeDecrypt
  - FirefoxDecrypt
  - WiPS
  

![ServersideScreenshot](https://github.com/benlarsendk/PS03/blob/master/screenshot.PNG "Screenshot of serverside")
### PS03 Server
This is the receiving end of the project. This would be the attackers machine, or a server. As for now it simply decrypt the incomming information and prints it to the console. This is a stand-alone solution and doesn't need any external libraries. 

### PS03 Victim
Is the client-side/victim-side of the project. It uses the two libraries to get saved Wifi-passwords as well as all saved passwords in chrome. This is also the soultion containing the logic for packing, encrypting and sending the data. **Remember to change the IP and port before using. 

### ChromeDecrypt
Is a library that locates and decrypts saved passwords from google chrome. It delivers the action-url, the username and of course the password. When done, it will raise an event with the decrypted data as an eventarg.

### FirefoxDecrypt
Exactly the same as ChromeDecrypt, it just works differently.

### WiPS
Is a library that makes use of the commandprompt to get saved WiFi-profiles, including the passwords. When done, it will raise an event with the data as an eventarg. The use of cmd wil not show any cmd-prompts on the victims machine.

### Credits

PS03 uses code from the following authors:

* [DPAPI] - For decrypting Chrome passwords
* [Mark Brittingham] - For simple AES encryption of data
* [SQLite] - For reading the Google Chrome Login Data file
* [3V1L] - For supplying some of the code for decrypting firefox passwords and username.


### Note before use
If you inted to use this software, be aware that you need to compile the libraries first, and include the .DLL's as references in the victim-side application. 

### Development

Want to contribute? Please do so. 
If you find an issue, please create it as an issue, and if you want you can fix it and make a pullrequest.

### Version

3.1.0

**CHANGELOG**
3.1.0
- PS03 can now handle Firefox user profiles. Tested on Firefox version 43.0.1.


3.0.2
- Fixed crashes when victim doesn't use chrome or have WLAN
- Fixed send timeout at victimside to exit quietly instead of crash
- General cleanup

   [DPAPI]: <http://www.obviex.com/samples/dpapi.aspx>
   [Mark Brittingham]: <http://stackoverflow.com/questions/165808/simple-two-way-encryption-for-c-sharp>
   [SQLite]: <https://www.sqlite.org/>
   [3V1L]: <http://xakfor.net/threads/c-firefox-36-password-cookie-recovery.12192/>
   

