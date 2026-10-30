"""Read-only API smoke checks. Usage: python tools/smoke_test.py http://localhost:5080"""
import json
import sys
import urllib.request
import urllib.error
base=sys.argv[1].rstrip('/') if len(sys.argv)>1 else 'http://localhost:5080'
def request(path, language=None):
    headers={'Accept':'application/json'}
    if language: headers['Accept-Language']=language
    try:
        response=urllib.request.urlopen(urllib.request.Request(base+path,headers=headers),timeout=20)
    except urllib.error.HTTPError as error:
        response=error
    return response.status,response.headers,json.loads(response.read())
status,_,languages=request('/api/localization/languages')
assert status==200 and {x['code'] for x in languages}=={'en-ZA','tn-ZA','nso-ZA'}
values={}
for culture in ['en-ZA','tn-ZA','nso-ZA']:
    status,headers,payload=request('/api/localization/resources',culture)
    assert status==200 and payload['culture']==culture
    assert headers['Content-Language']==culture
    values[culture]=payload['strings']
    for route in ['/api/settings','/api/admin/users','/api/admin/dashboard']:
        status,_,payload=request(route,culture)
        assert status==401 and payload['message']==values[culture]['Unauthorized']
assert values['tn-ZA']['Settings']!=values['en-ZA']['Settings']
assert values['nso-ZA']['Settings']!=values['en-ZA']['Settings']
status,_,payload=request('/api/localization/resources','fr-FR')
assert status==200 and payload['culture']=='en-ZA'
status,_,payload=request('/api/localization/resources?culture=nso-ZA&ui-culture=nso-ZA')
assert status==200 and payload['culture']=='nso-ZA'
print('PASS: language discovery, three resource cultures, Content-Language, translated 401 responses, unsupported-culture fallback and query selection.')
