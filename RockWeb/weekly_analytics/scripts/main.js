(function(){window.ENV={CLIENT_ID:"41598233398-itg6cmckds3a46e4sgolvq1ljsh3cchg.apps.googleusercontent.com",VIEW_ID:"ga:395366",REPORT_DAYS:7,PREVIOUS_LABEL:"week",REPORT_TITLE:"Weekly Website Report"}}).call(this),function(){var t,e,r,a,n,s,i,o,c,u;s=(new Date).toISOString().slice(0,10),i=location.search.slice(6),n={endDate:function(){return moment(i||s,"YYYY-MM-DD")},startDate:function(){return this.endDate().clone().subtract(ENV.REPORT_DAYS-1,"days")},compareEndDate:function(){return this.endDate().subtract(ENV.REPORT_DAYS,"days")},compareStartDate:function(){return this.startDate().subtract(ENV.REPORT_DAYS,"days")},endDateString:function(){return this.endDate().format("YYYY-MM-DD")},startDateString:function(){return this.startDate().format("YYYY-MM-DD")},compareEndDateString:function(){return this.compareEndDate().format("YYYY-MM-DD")},compareStartDateString:function(){return this.compareStartDate().format("YYYY-MM-DD")},title:function(){return ENV.REPORT_TITLE},subtitle:function(){return this.startDate().format("M/D")+" - "+this.endDate().format("M/D")},description:function(){return"compared to previous "+ENV.PREVIOUS_LABEL}},r={"start-date":n.startDateString(),"end-date":n.endDateString(),ids:ENV.VIEW_ID},a={"start-date":n.compareStartDateString(),"end-date":n.compareEndDateString(),ids:ENV.VIEW_ID},$("#date").val(n.endDateString()),$(".js-title").text(n.title()).append(" <small class='text-muted'>"+n.subtitle()+"</small>"),document.title=n.title()+" "+n.subtitle(),$(".js-description").text(n.description),u=function(){return $(".js-metric").each(function(e){var a,n;a=$(this),n=new gapi.analytics.report.Data({query:r}),n.set({query:{metrics:a.attr("data-metric")}}),n.on("success",function(t){var e;return e=Math.round(t.rows[0][0]),a.attr("data-per-day")&&(e=Math.round(e/ENV.REPORT_DAYS)),a.text(e)}),n.on("success",function(r){var n;return n=function(){return new t(a,r).execute()},o(n,300*e)}),n.execute()})},t=function(){function t(t,e){this.node=t,this.report=new gapi.analytics.report.Data({query:a}),this.report.set({query:{metrics:this.node.attr("data-metric")}}),this.base=e.rows[0][0],this.comparePercentage=this.node.data("compare-percentage")===!0,this.compareInvert=this.node.data("compare-invert")===!0,this.compareRaw=this.node.data("compare-raw")===!0,this.perDay=this.node.data("per-day")===!0}return t.prototype.execute=function(){return this.report.execute(),this.report.on("success",function(t){return function(e){return t.compare=e.rows[0][0],t.processReport()}}(this))},t.prototype.processReport=function(){return this._runCalculation(),this.node.parent().append(this._html())},t.prototype._runCalculation=function(){return this.perDay&&(this.base=this.base/ENV.REPORT_DAYS,this.compare=this.compare/ENV.REPORT_DAYS),this.comparePercentage||this.compareRaw?this.result=this.base-this.compare:this.result=(this.base-this.compare)/this.base*100},t.prototype._html=function(){return"<span class='metric-compare tag "+this._class()+"'>"+this._icon()+" "+this._content()+"</span>"},t.prototype._content=function(){return this.compareRaw?Math.round(this.result):Math.round(100*this.result)/100+"%"},t.prototype._isGood=function(){return this.compareInvert?this.result<0:this.result>0},t.prototype._class=function(){return this._isGood()?"tag-success":"tag-danger"},t.prototype._icon=function(){var t;return t=this.result>0?"▲":"▼","<span class='arrow'>"+t+"</span>"},t}(),e=function(){function t(t,e,r){this.element=t,this.baseQuery=e,this.compareQuery=r,this._prepareQueries()}return t.prototype.execute=function(){this.baseData.execute(),this.baseData.on("success",function(t){return function(e){return t.base=e,t.compareData.execute(),t.compareData.on("success",function(e){return t.compare=e,t._drawChart()})}}(this))},t.prototype._prepareQueries=function(){var t;return t={metrics:"ga:sessions",dimensions:"ga:date"},this.baseData=new gapi.analytics.report.Data({query:this.baseQuery}).set({query:t}),this.compareData=new gapi.analytics.report.Data({query:this.compareQuery}).set({query:t})},t.prototype._drawChart=function(){var t,e,r,a,n;return n=[["Date","Sessions","Previous Sessions"]],this.base.rows.map(function(t){return function(e,r){return n.push([moment(e[0]).format("ddd"),+e[1],+t.compare.rows[r][1]])}}(this)),e=this.base.rows.map(function(t){return+t[1]}),r=this.compare.rows.map(function(t){return+t[1]}),a=this.base.rows.map(function(t){return moment(t[0]).format("M/D")}),t={labels:a,datasets:[{label:"Current",data:e,backgroundColor:"rgba(151,187,205,0.5)",borderColor:"#3366cc"},{label:"Previous",data:r,backgroundColor:"rgba(150,150,150,.25)",borderColor:"rgba(150,150,150,.5)"}]},this.chart=new Chart(document.getElementById("multi-chart"),{type:"line",data:t,options:{responsive:!1}}),this._forceUpscalingBy(4)},t.prototype._forceUpscalingBy=function(t){var e,r,a,n,s;return r=this.chart.chart,a=r.ctx,e=r.canvas,s=e.width,n=e.height,e.height=n*t,e.width=s*t,a.scale(t,t)},t}(),gapi.analytics.ready(function(){var t,n,s,i,o;gapi.analytics.auth.authorize({container:"auth-button",clientid:ENV.CLIENT_ID}),s=new gapi.analytics.googleCharts.DataChart({reportType:"ga",query:{metrics:"ga:sessions",dimensions:"ga:date"},chart:{type:"LINE",container:"timeline",options:{width:"100%",height:"250"}}}),t=new gapi.analytics.googleCharts.DataChart({reportType:"ga",query:{metrics:"ga:sessions",dimensions:"ga:deviceCategory"},chart:{type:"PIE",container:"mobile-usage",options:{width:"100%",height:"250",legend:{position:"top",textStyle:{fontSize:9}}}}}),i=new gapi.analytics.googleCharts.DataChart({reportType:"ga",query:{metrics:"ga:sessions",dimensions:"ga:pageTitle",sort:"-ga:sessions","max-results":10},chart:{type:"TABLE",container:"top-pages",options:{width:"100%"}}}),o=new gapi.analytics.googleCharts.DataChart({reportType:"ga",query:{metrics:"ga:sessions",dimensions:"ga:socialNetwork",sort:"-ga:sessions",filters:"ga:socialNetwork!@not","max-results":10},chart:{type:"TABLE",container:"top-social",options:{width:"100%"}}}),n=new e("timeline",r,a),gapi.analytics.auth.on("success",function(e){var a;a=[function(){return n.execute()},function(){return t.set({query:r}).execute()},function(){return i.set({query:r}).execute()},function(){return o.set({query:r}).execute()},function(){return u()}],c(a,300)})}),c=function(t,e){var r,a,n,s,i,c;for(c=[],s=n=0,i=t.length;n<i;s=++n)a=t[s],r=s*e,c.push(o(a,r));return c},o=function(t,e){return setTimeout(function(){return t()},e)}}.call(this);