/// validation method for checking a password has been entered twice correctly, and is at least 5 characters
function CreateCustomValidationTypePassword()
{
	Ext.apply(Ext.form.VTypes, {
	   password: function(value, field)
	   {
		  var form = field.findParentByType('form'); 
		  var f; 
		  if (form && form.getForm() && (f = form.getForm().findField(field.otherPasswordField))) 
		  {
			 this.passwordText = 'Confirmation does not match your first password entry.';
			 if (f.getValue().length > 0 && value != f.getValue())
			 {
				return false;
			 }
		  }
	 
		  this.passwordText = 'Passwords must be at least 5 characters';
	 
		  var hasLength = (value.length >= 5);
	 
		  return (hasLength);
	   },
	 
	   passwordText: 'Passwords must be at least 5 characters',
	});    
}