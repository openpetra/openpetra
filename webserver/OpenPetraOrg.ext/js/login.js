function XmlExtractJSONResponse(response)
{
    var xml = response.responseXML;
    var stringDataNode = xml.getElementsByTagName('string')[0];
    if(stringDataNode){
        jsonString = stringDataNode.firstChild.data;
        jsonData = Ext.util.JSON.decode(jsonString);
        return jsonData;
    }
}


Ext.onReady(function(){
    Ext.QuickTips.init();

    // use our own blank image to avoid a call home
    // Ext.BLANK_IMAGE_URL = 'resources/images/default/s.gif';

    // Create a variable to hold our EXT Form Panel. 
    // Assign various config options as seen.	 
    var login = new Ext.FormPanel({ 
        labelWidth:80,
        frame:true, 
        title:'Please Login', 
        defaultType:'textfield',
        monitorValid:true,
        // Specific attributes for the text fields for username / password. 
        // The "name" attribute defines the name of variables sent to the server.
        items:[{ 
                id: 'Username',
                fieldLabel:'Username', 
                allowBlank:false 
            },{ 
                id: 'Password',
                fieldLabel:'Password', 
                inputType:'password', 
                allowBlank:false 
            }],
 
        // All the magic happens after the user clicks the button     
        buttons:[{ 
                text:'Login',
                formBind: true,	 
                // Function that fires when user clicks the button 
                handler:function(){
                    Ext.Ajax.request({
                        url: '/server.asmx/Login',
                        params:{
                            username: Ext.getCmp('Username').getValue().toUpperCase(),
                            password: Ext.getCmp('Password').getValue()
                        },
                        success: function(response){
                        
                            // response contains: <?xml version="1.0" encoding="utf-8"?><string xmlns="http://tempuri.org/">[true]</string>
                            jsonData = XmlExtractJSONResponse(response);
                            resultMessage = jsonData;
                            if (resultMessage == "true")
                            {
                                var redirect = 'test.asp'; 
                                window.location = redirect;
                            }
                            else
                            {
                                Ext.Msg.alert('Login Failed!', "Wrong username or password"); 
                            }
                            
                        },
                        failure: function(response){
                            if(response.failureType == 'server'){ 
                                obj = Ext.util.JSON.decode(response.response.responseText); 
                                Ext.Msg.alert('Login Failed!', obj.errors.reason); 
                            }else{ 
                                Ext.Msg.alert('Warning!', 'Authentication server is unreachable : ' + response.response.responseText); 
                            } 
                            login.getForm().reset(); 
                        }
                    });                
                } 
            }] 
    });
 
 
    // This just creates a window to wrap the login form. 
    // The login object is passed to the items collection.       
    var win = new Ext.Window({
        layout:'fit',
        width:300,
        height:150,
        closable: false,
        resizable: false,
        plain: true,
        border: false,
        items: [login]
    });
    win.show();
});