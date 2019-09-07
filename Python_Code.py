# -*- encoding: utf8 -*-
import requests
import json
def sendNotification(user):
    target = "https://fcm.googleapis.com/fcm/send"
    header = {
        "Authorization" : "key=AAAA0JeDn1U:APA91bG2TkugUNq_xg1GlzvpZ5daJvmxpbzupydrWAcM3C6HA9VqtXscgMUShGgWrJE453r8DonmgZmcZ8TPL4VRrLIl8bcAdNTFINN6g7__yf77OPSfS42t7JeTBSNU_aSqj-REbs-k",
        "Content-Type" : "application/json"
        }
    payload = { "notification": {
        "title": "Notification Title to send",
        "text": "Notification Body to display",
         "click_action": ""
      },
        "android":{
      "ttl":"36500s"
    },
        "data": {
        "keyname": "any value "
        },
      "to" : user
    } 
    x = requests.post(target, data = json.dumps(payload), headers=header)
    print x.content

users = ['device_token']
for i in users:
    sendNotification(i)
