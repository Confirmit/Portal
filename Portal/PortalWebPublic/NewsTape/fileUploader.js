//---------------------------------------------------------------------

function FileUploader(idParentElem, strElemName, clientObjectName)
{    
    this.idParentElem = idParentElem;
    this.idElemName = strElemName;
    
    this.clientObjectName = clientObjectName;
    this.currentID = 0;
    this.initFileUploader();
}

FileUploader.prototype.initFileUploader = function()
{
    var strElemName = this.idElemName + "_" + this.currentID;
    var fileElem = $get(strElemName);
    
    if (fileElem == null) {  
        /* images */
        divElem = document.createElement('div');
        divElem.id = strElemName + "_div";                
        divElem.className = "attachfile-hide-img";  
        divElem.innerHTML = this.generateInnerHTML(strElemName);
        /* end creating div container of images */
        
        /* input type='file' */
        fileElem = document.createElement('input');
        fileElem.type = 'file';
        fileElem.id = strElemName;
        fileElem.name = strElemName;
        fileElem.size = '40';
        fileElem.className = "control-label";
        fileElem.uploaderObject = this;
        
        fileElem.onchange = function()
        {
            this.uploaderObject.onChange(this);
        };
        /* end creating of input type = 'file' */
                           
        $get(this.idParentElem).appendChild(divElem);        
        $get(this.idParentElem).appendChild(fileElem);   
        this.currentID++;
    }
    else 
    {
        this.currentID++;
        this.initFileUploader();        
    }
}

FileUploader.prototype.onChange = function(fileElem)
{
    fileElem.className = "attachfile-hide";    
    $get(fileElem.id + "_text").innerHTML = fileElem.value;
    $get(fileElem.id + "_div").className = "";
    
    if (fileElem.value != null)
        this.initFileUploader();
}

FileUploader.prototype.onDelete = function(strElemName)
{
    var divChild = $get(strElemName + "_div");
    var fileChild = $get(strElemName);
    
    $get(this.idParentElem).removeChild(divChild);
    $get(this.idParentElem).removeChild(fileChild);
}

FileUploader.prototype.generateInnerHTML = function(strElemName)
{
    var strInnerHtml = " <div style='float: left;' class='attachfile-left'></div>";
    strInnerHtml += " <div style='float: left;' class='attachfile-text' id = '" + strElemName + "_text'> </div>";
    strInnerHtml += " <div style='float: left;' class='attachfile-right'></div>";
    strInnerHtml += " <div style='float: left;' class='attachfile-text'>";
        
    var strClickDeleteEvent = this.clientObjectName
                        + ".onDelete('" 
                        + strElemName 
                        + "');";
                                             
    strInnerHtml += ' <a class="control-hyperlink" onclick="' 
                    + strClickDeleteEvent 
                    + '">Delete</a>';
    strInnerHtml += " </div>";
    
    return strInnerHtml;
}
