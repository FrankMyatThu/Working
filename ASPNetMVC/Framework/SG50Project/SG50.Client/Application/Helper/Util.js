function ToJavaScriptDate(value) {
    console.log("ToJavaScriptDate " + value);
    var pattern = /Date\(([^)]+)\)/;
    console.log("pattern " + pattern);
    var results = pattern.exec(value);
    console.log("results " + results);
    var dt = new Date(parseFloat(results[1]));
    return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
}

function getJsonDate(value) {
    console.log("getJsonDate " + value);
    var dt = new Date(value);
    console.log("dt " + dt);
    var ticks = ((dt.getTime() * 10000) + 621355968000000000);
    console.log("ticks " + ticks);
    var JsonDateValue = "/Date(" + ticks + ")/";
    console.log("JsonDateValue " + JsonDateValue);
    return JsonDateValue;
}