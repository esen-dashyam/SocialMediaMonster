(function(a){a.fn.SharkDevTreeView=function(c){var d=new function(h){this.AfterSelectHandler=c.AfterSelectHandler;this.DataOnClient=c.DataOnClient;this.AutoCompleteHandler=c.AutoCompleteHandler;this.ControlId=h}(this.attr("id"));var f=d.ControlId+"_rootUl";var e="hiddenUl";var b="Expander_0";ProgressBar=function(p){var j=1;var m="progressCell";var h=false;var o=a("#"+p+" #progressParent")[0];var n=a("#"+p+" #progressParent > span");var k;this.Start=function(){a(o).css("display","inline");var i=this;k=setInterval(function(){i.Loop()},300)};this.Loop=function(){if(j>3){this.ResetControls();j=1}else{this.ResetControls();a(n).each(function(q,i){var r=m+j;if(i.id==r){a(i).css("padding-top","2px");a(i).css("padding-bottom","2px");a(i).css("padding-left","2px");return false}});++j}};this.Stop=function(){a(o).css("display","");j=1;h=false;this.ResetControls();clearInterval(k)};this.ResetControls=function(){a(n).css("padding-top","0px");a(n).css("padding-bottom","0px");a(n).css("padding-left","0px")}};ScrollToElement=function(m,k){a("#"+k.ControlId+"_rootUl").removeClass("hiddenUl");var r=a("#"+k.ControlId+" #Content_"+m).parents("li");for(var n=0;n<r.length;n++){a(r[n]).removeClass(e).removeClass("contentSelected")}var j=r[0];for(var n=0;;n++){j=a(j).parent().closest("li").get(0);var o=a(j).find("a");if(o.length>0){a(o[0]).removeClass("expand");a(o[0]).addClass("collapse")}if(j==undefined){break}}var p=k.ControlId+"_header";var h=a("#"+p).offset().top+20;var q=k.ControlId+"_tree";a("#"+q).animate({scrollTop:0,scrollLeft:0},0,function(){if(a("#"+k.ControlId+" #Content_"+m).get(0)!=undefined){var t=a("#"+k.ControlId+" #Content_"+m).offset().top;var s=a("#"+k.ControlId+" #Content_"+m).offset().left;var i=a("#"+k.ControlId+" #Content_"+m).offset().right;a("#"+q).animate({scrollTop:t-h,scrollRight:i},200,function(){SelectElement(a("#"+k.ControlId+" #Content_"+m),k)})}})};SelectElement=function(i,j){if(a("#"+j.ControlId+' a[id="Expander_0"]').hasClass("expand")){a("#"+j.ControlId+' a[id="Expander_0"]').removeClass("expand");a("#"+j.ControlId+' a[id="Expander_0"]').addClass("collapse")}var h=a(i).attr("obj");a("#"+j.ControlId+" span[class=contentSelected]").parent().removeClass("hover2");a("#"+j.ControlId+" span[class=contentSelected]").removeClass("contentSelected");var m=a(i).text();a(i).text("");a(i).addClass("hover2");a(i).append('<span id="contentInside" class="contentSelected">'+m+"</span>");var k=a(i).prev('a[id^="Expander"]');if(j.AfterSelectHandler!=null){j.AfterSelectHandler(JSON.parse(h))}};BindMethods=function(){return function(i,h){var k;a("#"+h.ControlId+"_autoCompleteInput").autocomplete({source:function(n,m){if(h.DataOnClient){var q=JSON.parse(a("#"+h.ControlId+"_JsonData").text());m(a.map(q,function(p){if(p.Term.toLowerCase().indexOf(n.term.toLowerCase())==0){return{label:p.Term,id:p.Id}}}))}else{var o=(g.AutoCompleteHandler.indexOf("?")>0)?"&q=":"?q=";a.ajax({type:"POST",url:g.AutoCompleteHandler+o+l.term,dataType:"json",contentType:"application/json; charset=utf-8",success:function(p){var r=p;m(a.map(r,function(s){return{label:s.Term,id:s.Id}}))}})}},appendTo:a("#"+h.ControlId+"_autoCompleteInput").parent(),minLength:2,messages:{noResults:"",results:function(m,n){}},response:function(m,n){k.Stop();k=null},search:function(m,n){k=new ProgressBar(h.ControlId);k.Start()},select:function(m,n){ScrollToElement(n.item.id,h)}});var j=a("#"+i+' span[id^="Content"]');a(j).mouseover(function(){a(this).addClass("hover")});a(j).mouseleave(function(){a(this).removeClass("hover")});a(j).click(function(){SelectElement(this,h)});a("#"+i+' a[id^="Expander"]').click(function(n){var m=a(this).parents("li").get(0);if(a(m).hasClass(e)==true){a(m).removeClass(e)}else{a(m).addClass(e)}if(a(this).hasClass("expand")){a(this).removeClass("collapse");a(this).removeClass("expand");a(this).addClass("collapse")}else{a(this).removeClass("collapse");a(this).removeClass("expand");a(this).attr("class","");a(this).addClass("expand")}});a("#"+h.ControlId+' a[id="Expander_0"]').off("click");a("#"+h.ControlId+' a[id^="Expander_0"]').click(function(m){if(a("#"+f).hasClass(e)==true){a("#"+f).removeClass(e);a(this).attr("class","");a(this).addClass("collapse")}else{a(this).attr("class","");a(this).addClass("expand");a("#"+f).addClass(e)}})}(f,d)}()}})(jQuery);