Ext.onReady(function(){
    Ext.QuickTips.init();
 
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
                            if (Ext.util.Format.trim(response.responseText).toLowerCase() == "true")
                            {
                                var redirect = 'test.asp'; 
                                window.location = redirect;
                            }
                            else
                            {
                                Ext.Msg.alert('Login Failed!', ">" + Ext.util.Format.trim(response.responseText).toLowerCase() + ">"); 
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