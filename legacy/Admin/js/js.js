// JScript File

function clickDefaButton(e, buttonid)
{ 
	var bt = document.getElementById(buttonid); 
	if (typeof bt == 'object')
	{ 
			if(navigator.appName.indexOf("Netscape")>(-1))
			{ 
				if (e.keyCode == 13){ 
						bt.click(); 
						return false; 
				} 
			} 
			if (navigator.appName.indexOf("Microsoft Internet Explorer")>(-1)){ 
				if (event.keyCode == 13){ 
						bt.click(); 
						return false; 
				} 
			} 
	} 
}
Function.prototype.startsWith = function () { return false; }; 
