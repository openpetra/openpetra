
/*
 * Javascript "SOAP Client" library
 *
 * @version: 2.4 - 2007.12.21
 * @author: Matteo Casati - http://www.guru4.net/
 *
 */

/*
 * Qooxdoo integration by Burak Arslan (burak.arslan-qx@arskom.com.tr)
 */

qx.Class.define("openpetraorg.soap.Parameters", { extend : qx.core.Object
    ,construct : function() {
        this.__pl={};
    }

    ,members : {
        __pl : null

        ,__serialize : function(o) {
            var s = "";
            switch(typeof(o)) {

            case "string":
                s += o.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
                break;

            case "number":
            case "boolean":
                s += o.toString();
                break;

            case "object":
                if (o == null) {
                    return "";
                }
                // Date
                else if(o.constructor.toString().indexOf("function Date()") > -1) {
                    var year = o.getFullYear().toString();
                    var month = (o.getMonth() + 1).toString();

                    month = (month.length == 1) ? "0" + month : month;

                    var date = o.getDate().toString();
                    date = (date.length == 1) ? "0" + date : date;

                    var hours = o.getHours().toString();
                    hours = (hours.length == 1) ? "0" + hours : hours;

                    var minutes = o.getMinutes().toString();
                    minutes = (minutes.length == 1) ? "0" + minutes : minutes;

                    var seconds = o.getSeconds().toString();
                    seconds = (seconds.length == 1) ? "0" + seconds : seconds;

                    var milliseconds = o.getMilliseconds().toString();
                    var tzminutes = Math.abs(o.getTimezoneOffset());
                    var tzhours = 0;

                    while(tzminutes >= 60) {
                        tzhours++;
                        tzminutes -= 60;
                    }
                    tzminutes = (tzminutes.toString().length == 1) ? "0" + tzminutes.toString() : tzminutes.toString();
                    tzhours = (tzhours.toString().length == 1) ? "0" + tzhours.toString() : tzhours.toString();
                    var timezone = ((o.getTimezoneOffset() < 0) ? "+" : "-") + tzhours + ":" + tzminutes;
                    s += year + "-" + month + "-" + date + "T" + hours + ":" + minutes + ":" + seconds + "." + milliseconds + timezone;
                }
                // Array
                else if(o.constructor.toString().indexOf("function Array()") > -1) {
                    for(var p in o) {
                        if(!isNaN(p)) { // linear array
                            (/function\s+(\w*)\s*\(/ig).exec(o[p].constructor.toString());

                            var type = RegExp.$1;
                            switch(type) {
                            case "":
                                type = typeof(o[p]);
                                break;

                            case "String":
                                type = "string";
                                break;

                            case "Number":
                                type = "int";
                                break;

                            case "Boolean":
                                type = "bool";
                                break;

                            case "Date":
                                type = "DateTime";
                                break;
                            }
                            s += "<" + type + ">" + this.__serialize(o[p]) + "</" + type + ">"
                        }
                        else {   // associative array
                            s += "<" + p + ">" + this.__serialize(o[p]) + "</" + p + ">"
                        }
                    }
                }
                else { // Object or custom function
                    for(var c in o.simple_object) {
                        s += "<" + c + ">" + this.__serialize(o.simple_object[c]) + "</" + c + ">";
                    }
                }
                break;

            case "function":
                break;

            default:
                break;
            }
            return s;
        }

        ,add : function(name, value) {
            this.__pl[name] = value;
            return this;
        }

        ,toXml : function() {
            var xml = "";

            for(var p in this.__pl) {
                switch(typeof(this.__pl[p])) {
                case "string":
                case "number":
                case "boolean":
                case "object":
                    xml += "<" + p + ">" + this.__serialize(this.__pl[p]) + "</" + p + ">";
                    break;

                default:
                    this.warn("variable '" + p + "' with type '"+typeof(this.__pl[p])+"' ignored");
                }
            }

            return xml;
        }
    }
});

