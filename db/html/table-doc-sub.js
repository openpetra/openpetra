/********************************************************************************
Copyright (C) 1999 Thomas Brattli
This script is made by and copyrighted to Thomas Brattli at www.bratta.com
Visit for more great scripts. This may be used freely as long as this msg is intact!
I will also appriciate any links you could give me.
********************************************************************************/
//Default browsercheck, added to all scripts!
function checkBrowser(){
    this.ver=navigator.appVersion
    this.dom=document.getElementById?1:0
    this.ie5=(this.ver.indexOf("MSIE")>-1 && this.dom)?1:0;
    this.ie4=(document.all && !this.dom)?1:0;
    this.ns5=(this.dom && parseInt(this.ver) >= 5) ?1:0;
    this.ns4=(document.layers && !this.dom)?1:0;
    this.bw=(this.ie5 || this.ie4 || this.ns4 || this.ns5)
    return this
}
var bw=new checkBrowser()
/***************************************************************************************
Variables to set:
***************************************************************************************/
var fromX = 10; //How much from the actual mouse X should the description box appear?
var fromY = -20;//How much from the actual mouse Y should the description box appear?

//To set the font size, font type, border color or remove the border or whatever,
//change the clDescription class in the stylesheet.

//Makes crossbrowser object.
function makeObj(obj){
    this.css=bw.dom? document.getElementById(obj).style:bw.ie4?document.all[obj].style:bw.ns4?document.layers[obj]:0;
    this.wref=bw.dom? document.getElementById(obj):bw.ie4?document.all[obj]:bw.ns4?document.layers[obj].document:0;
    this.writeIt=b_writeIt;
    return this
}
function b_writeIt(text) {
    if(bw.ns4) {
        this.wref.write(text);
        this.wref.close()
    } else {
        this.wref.innerHTML=text;
    }
}

//Capturing mousemove
var descx,descy;
function popmousemove(e) {
  var tempX, tempY;
  if (bw.ie5) { // grab the x-y pos.s if browser is IE
    tempX = event.clientX + document.body.scrollLeft
    tempY = event.clientY + document.body.scrollTop
  } else {  // grab the x-y pos.s if browser is NS
    tempX = e.pageX
    tempY = e.pageY
  }
  // catch possible negative values in NS4
  if (tempX < 0){tempX = 0}
  if (tempY < 0){tempY = 0}
  descx=tempX;
  descy=tempY;
}

//Initiates page
var isLoaded = false;
var oDesc;
function popupInit() {
    oDesc=new makeObj('divDescription');
    if ( bw.ns4) document.captureEvents(Event.MOUSEMOVE);
    document.onmousemove=popmousemove;
    isLoaded=true;
}
//Shows the messages
function popup( txt) {
    if ( isLoaded) {
        oDesc.writeIt('<span class="clDescription">'+txt+'</span>');
        if ( bw.ie5) descy=descy+document.body.scrollTop;
        oDesc.css.left=descx+fromX; oDesc.css.top=descy+fromY;
        oDesc.css.visibility='visible';
    }
}
//Hides it
function popout() {
    if ( isLoaded) oDesc.css.visibility='hidden'
}
