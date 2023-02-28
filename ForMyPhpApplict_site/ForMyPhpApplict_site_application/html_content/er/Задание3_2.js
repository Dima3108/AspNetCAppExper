var d_control = document.getElementById("js_content");
document.createElement("table");
var t_=document.querySelector("table");
t_.className ="table_st";
var df=document.createDocumentFragment();
for(var i=0;i<5;i++){
    var td = document.createElement("td");
    var tr = document.createElement("tr");
    td.textContent = i;
    if(i%2==0){
        td.className="td1";
    }
    else{
        td.className="td2";
    }
    tr.appendChild(td)
    df.appendChild(tr);
}