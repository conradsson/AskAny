/*
@license
dhtmlxScheduler.Net v.3.3.23 Professional Evaluation

This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
Scheduler.plugin(function(e){!function(){e._grid={sort_rules:{"int":function(e,t,a){return 1*a(e)<1*a(t)?1:-1},str:function(e,t,a){return a(e)<a(t)?1:-1},date:function(e,t,a){return new Date(a(e))<new Date(a(t))?1:-1}},_getObjName:function(e){return"grid_"+e},_getViewName:function(e){return e.replace(/^grid_/,"")}}}(),e.createGridView=function(t){function a(e){return!(void 0!==e&&(1*e!=e||0>e))}var n=t.name||"grid",i=e._grid._getObjName(n);e.config[n+"_start"]=t.from||new Date(0),e.config[n+"_end"]=t.to||new Date(9999,1,1),
e[i]=t,e[i].defPadding=8,e[i].columns=e[i].fields,e[i].unit=t.unit||"month",e[i].step=t.step||1,delete e[i].fields;for(var r=e[i].columns,l=0;l<r.length;l++)a(r[l].width)&&(r[l].initialWidth=r[l].width),a(r[l].paddingLeft)||delete r[l].paddingLeft,a(r[l].paddingRight)||delete r[l].paddingRight;e[i].select=void 0===t.select?!0:t.select,void 0===e.locale.labels[n+"_tab"]&&(e.locale.labels[n+"_tab"]=e[i].label||e.locale.labels.grid_tab),e[i]._selected_divs=[],e.date[n+"_start"]=function(a){return e.date[t.unit+"_start"]?e.date[t.unit+"_start"](a):a;

},e.date["add_"+n]=function(t,a){return e.date.add(t,a*e[i].step,e[i].unit)},e.templates[n+"_date"]=function(t,a){return e.templates.day_date(t)+" - "+e.templates.day_date(a)},e.templates[n+"_full_date"]=function(t,a,i){return e.isOneDayEvent(i)?this[n+"_single_date"](t):e.templates.day_date(t)+" &ndash; "+e.templates.day_date(a)},e.templates[n+"_single_date"]=function(t){return e.templates.day_date(t)+" "+this.event_date(t)},e.templates[n+"_field"]=function(e,t){return t[e]},e.attachEvent("onTemplatesReady",function(){
e.attachEvent("onDblClick",function(t,a){return this._mode==n?(e._click.buttons.details(t),!1):!0}),e.attachEvent("onClick",function(t,a){return this._mode==n&&e[i].select?(e._grid.unselectEvent("",n),e._grid.selectEvent(t,n,a),!1):!0});var t=e.render_data;e.render_data=function(a){return this._mode!=n?t.apply(this,arguments):void e._grid._fill_grid_tab(i)};var a=e.render_view_data;e.render_view_data=function(){var t=e._els.dhx_cal_data[0].lastChild;return this._mode==n&&t&&(e._grid._gridScrollTop=t.scrollTop),
a.apply(this,arguments)}}),e[n+"_view"]=function(t){if(e._grid._sort_marker=null,delete e._gridView,e._grid._gridScrollTop=0,e._rendered=[],e[i]._selected_divs=[],t){var a=null,r=null,l=e[i];l.paging?(a=e.date[n+"_start"](new Date(e._date)),r=e.date["add_"+n](a,1)):(a=e.config[n+"_start"],r=e.config[n+"_end"]),e._min_date=a,e._max_date=r,e._grid.set_full_view(i);var d="";+a>+new Date(0)&&+r<+new Date(9999,1,1)&&(d=e.templates[n+"_date"](a,r)),e._els.dhx_cal_date[0].innerHTML=d,e._gridView=i}}},e.dblclick_dhx_grid_area=function(){
!this.config.readonly&&this.config.dblclick_create&&this.addEventNow()},e._click.dhx_cal_header&&(e._old_header_click=e._click.dhx_cal_header),e._click.dhx_cal_header=function(t){if(e._gridView){var a=t||window.event,n=e._grid.get_sort_params(a,e._gridView);e._grid.draw_sort_marker(a.originalTarget||a.srcElement,n.dir),e.clear_view(),e._grid._fill_grid_tab(e._gridView,n)}else if(e._old_header_click)return e._old_header_click.apply(this,arguments)},e._grid.selectEvent=function(t,a,n){if(e.callEvent("onBeforeRowSelect",[t,n])){
var i=e._grid._getObjName(a);e.for_rendered(t,function(t){t.className+=" dhx_grid_event_selected",e[i]._selected_divs.push(t)}),e._select_id=t}},e._grid._unselectDiv=function(e){e.className=e.className.replace(/ dhx_grid_event_selected/,"")},e._grid.unselectEvent=function(t,a){var n=e._grid._getObjName(a);if(n&&e[n]._selected_divs)if(t){for(var i=0;i<e[n]._selected_divs.length;i++)if(e[n]._selected_divs[i].getAttribute("event_id")==t){e._grid._unselectDiv(e[n]._selected_divs[i]),e[n]._selected_divs.slice(i,1);

break}}else{for(var i=0;i<e[n]._selected_divs.length;i++)e._grid._unselectDiv(e[n]._selected_divs[i]);e[n]._selected_divs=[]}},e._grid.get_sort_params=function(t,a){var n=t.originalTarget||t.srcElement,i="desc",r=e._getClassName(n);"dhx_grid_view_sort"==r&&(n=n.parentNode),r=e._getClassName(n),-1==r.indexOf("dhx_grid_sort_asc")&&(i="asc");for(var l=0,d=0;d<n.parentNode.childNodes.length;d++)if(n.parentNode.childNodes[d]==n){l=d;break}var o=null;if(e[a].columns[l].template){var s=e[a].columns[l].template;

o=function(e){return s(e.start_date,e.end_date,e)}}else{var _=e[a].columns[l].id;"date"==_&&(_="start_date"),o=function(e){return e[_]}}var c=e[a].columns[l].sort;return"function"!=typeof c&&(c=e._grid.sort_rules[c]||e._grid.sort_rules.str),{dir:i,value:o,rule:c}},e._grid.draw_sort_marker=function(t,a){"dhx_grid_view_sort"==t.className&&(t=t.parentNode),e._grid._sort_marker&&(e._grid._sort_marker.className=e._grid._sort_marker.className.replace(/( )?dhx_grid_sort_(asc|desc)/,""),e._grid._sort_marker.removeChild(e._grid._sort_marker.lastChild)),
t.className+=" dhx_grid_sort_"+a,e._grid._sort_marker=t;var n="<div class='dhx_grid_view_sort' style='left:"+(+t.style.width.replace("px","")-15+t.offsetLeft)+"px'>&nbsp;</div>";t.innerHTML+=n},e._grid.sort_grid=function(t){var t=t||{dir:"desc",value:function(e){return e.start_date},rule:e._grid.sort_rules.date},a=e.get_visible_events();return a.sort("desc"==t.dir?function(e,a){return t.rule(e,a,t.value)}:function(e,a){return-t.rule(e,a,t.value)}),a},e._grid.set_full_view=function(t){if(t){var a=(e.locale.labels,
e._grid._print_grid_header(t));e._els.dhx_cal_header[0].innerHTML=a,e._table_view=!0,e.set_sizes()}},e._grid._calcPadding=function(t,a){var n=(void 0!==t.paddingLeft?1*t.paddingLeft:e[a].defPadding)+(void 0!==t.paddingRight?1*t.paddingRight:e[a].defPadding);return n},e._grid._getStyles=function(e,t){for(var a=[],n="",i=0;t[i];i++)switch(n=t[i]+":",t[i]){case"text-align":e.align&&a.push(n+e.align);break;case"vertical-align":e.valign&&a.push(n+e.valign);break;case"padding-left":void 0!==e.paddingLeft&&a.push(n+(e.paddingLeft||"0")+"px");

break;case"padding-right":void 0!==e.paddingRight&&a.push(n+(e.paddingRight||"0")+"px")}return a},e._grid._fill_grid_tab=function(t,a){for(var n=(e._date,e._grid.sort_grid(a)),i=e[t].columns,r="<div>",l=-2,d=0;d<i.length;d++){var o=e._grid._calcPadding(i[d],t);l+=i[d].width+o,d<i.length-1&&(r+="<div class='dhx_grid_v_border' style='left:"+l+"px'></div>")}r+="</div>",r+="<div class='dhx_grid_area'><table>";for(var d=0;d<n.length;d++)r+=e._grid._print_event_row(n[d],t);r+="</table></div>",e._els.dhx_cal_data[0].innerHTML=r,
e._els.dhx_cal_data[0].lastChild.scrollTop=e._grid._gridScrollTop||0;var s=e._els.dhx_cal_data[0].getElementsByTagName("tr");e._rendered=[];for(var d=0;d<s.length;d++)e._rendered[d]=s[d]},e._grid._print_event_row=function(t,a){var n=[];t.color&&n.push("background:"+t.color),t.textColor&&n.push("color:"+t.textColor),t._text_style&&n.push(t._text_style),e[a].rowHeight&&n.push("height:"+e[a].rowHeight+"px");var i="";n.length&&(i="style='"+n.join(";")+"'");for(var r=e[a].columns,l=e.templates.event_class(t.start_date,t.end_date,t),d="<tr class='dhx_grid_event"+(l?" "+l:"")+"' event_id='"+t.id+"' "+i+">",o=e._grid._getViewName(a),s=["text-align","vertical-align","padding-left","padding-right"],_=0;_<r.length;_++){
var c;c=r[_].template?r[_].template(t.start_date,t.end_date,t):"date"==r[_].id?e.templates[o+"_full_date"](t.start_date,t.end_date,t):"start_date"==r[_].id||"end_date"==r[_].id?e.templates[o+"_single_date"](t[r[_].id]):e.templates[o+"_field"](r[_].id,t);var u=e._grid._getStyles(r[_],s),h=r[_].css?' class="'+r[_].css+'"':"";d+="<td style='width:"+r[_].width+"px;"+u.join(";")+"' "+h+">"+c+"</td>"}return d+="<td class='dhx_grid_dummy'></td></tr>"},e._grid._print_grid_header=function(t){for(var a="<div class='dhx_grid_line'>",n=e[t].columns,i=[],r=n.length,l=e._obj.clientWidth-2*n.length-20,d=0;d<n.length;d++){
var o=1*n[d].initialWidth;isNaN(o)||""===n[d].initialWidth||null===n[d].initialWidth||"boolean"==typeof n[d].initialWidth?i[d]=null:(r--,l-=o,i[d]=o)}for(var s=Math.floor(l/r),_=["text-align","padding-left","padding-right"],c=0;c<n.length;c++){var u=i[c]?i[c]:s;n[c].width=u-e._grid._calcPadding(n[c],t);var h=e._grid._getStyles(n[c],_);a+="<div style='width:"+(n[c].width-1)+"px;"+h.join(";")+"'>"+(void 0===n[c].label?n[c].id:n[c].label)+"</div>"}return a+="</div>"}});