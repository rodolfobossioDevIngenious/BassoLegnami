/*! For license information please see liquid.js.LICENSE.txt */
define("vs/basic-languages/liquid/liquid",["require","require"],(e=>(()=>{var t,i,r=Object.create,n=Object.defineProperty,o=Object.getOwnPropertyDescriptor,l=Object.getOwnPropertyNames,a=Object.getPrototypeOf,d=Object.prototype.hasOwnProperty,s=e=>n(e,"__esModule",{value:!0}),u=(t=function(t){if(void 0!==e)return e.apply(this,arguments);throw new Error('Dynamic require of "'+t+'" is not supported')},void 0!==e?e:"undefined"!=typeof Proxy?new Proxy(t,{get:(t,i)=>(void 0!==e?e:t)[i]}):t),c=(e,t,i,r)=>{if(t&&"object"==typeof t||"function"==typeof t)for(let a of l(t))!d.call(e,a)&&(i||"default"!==a)&&n(e,a,{get:()=>t[a],enumerable:!(r=o(t,a))||r.enumerable});return e},m=(e,t)=>c(s(n(null!=e?r(a(e)):{},"default",!t&&e&&e.__esModule?{get:()=>e.default,enumerable:!0}:{value:e,enumerable:!0})),e),p=(e=>(t,i)=>e&&e.get(t)||(i=c(s({}),t,1),e&&e.set(t,i),i))("undefined"!=typeof WeakMap?new WeakMap:0),f=()=>{return i||(e=i={exports:{}},t=m(u("vs/editor/editor.api")),e.exports=t),i.exports;var e,t},w={};((e,t)=>{for(var i in t)n(e,i,{get:t[i],enumerable:!0})})(w,{conf:()=>q,language:()=>h});var g={};c(g,m(f()));var b=["area","base","br","col","embed","hr","img","input","keygen","link","menuitem","meta","param","source","track","wbr"],q={wordPattern:/(-?\d*\.\d\w*)|([^\`\~\!\@\$\^\&\*\(\)\=\+\[\{\]\}\\\|\;\:\'\"\,\.\<\>\/\s]+)/g,brackets:[["\x3c!--","--\x3e"],["<",">"],["{{","}}"],["{%","%}"],["{","}"],["(",")"]],autoClosingPairs:[{open:"{",close:"}"},{open:"%",close:"%"},{open:"[",close:"]"},{open:"(",close:")"},{open:'"',close:'"'},{open:"'",close:"'"}],surroundingPairs:[{open:"<",close:">"},{open:'"',close:'"'},{open:"'",close:"'"}],onEnterRules:[{beforeText:new RegExp(`<(?!(?:${b.join("|")}))(\\w[\\w\\d]*)([^/>]*(?!/)>)[^<]*$`,"i"),afterText:/^<\/(\w[\w\d]*)\s*>$/i,action:{indentAction:g.languages.IndentAction.IndentOutdent}},{beforeText:new RegExp(`<(?!(?:${b.join("|")}))(\\w[\\w\\d]*)([^/>]*(?!/)>)[^<]*$`,"i"),action:{indentAction:g.languages.IndentAction.Indent}}]},h={defaultToken:"",tokenPostfix:"",builtinTags:["if","else","elseif","endif","render","assign","capture","endcapture","case","endcase","comment","endcomment","cycle","decrement","for","endfor","include","increment","layout","raw","endraw","render","tablerow","endtablerow","unless","endunless"],builtinFilters:["abs","append","at_least","at_most","capitalize","ceil","compact","date","default","divided_by","downcase","escape","escape_once","first","floor","join","json","last","lstrip","map","minus","modulo","newline_to_br","plus","prepend","remove","remove_first","replace","replace_first","reverse","round","rstrip","size","slice","sort","sort_natural","split","strip","strip_html","strip_newlines","times","truncate","truncatewords","uniq","upcase","url_decode","url_encode","where"],constants:["true","false"],operators:["==","!=",">","<",">=","<="],symbol:/[=><!]+/,identifier:/[a-zA-Z_][\w]*/,tokenizer:{root:[[/\{\%\s*comment\s*\%\}/,"comment.start.liquid","@comment"],[/\{\{/,{token:"@rematch",switchTo:"@liquidState.root"}],[/\{\%/,{token:"@rematch",switchTo:"@liquidState.root"}],[/(<)([\w\-]+)(\/>)/,["delimiter.html","tag.html","delimiter.html"]],[/(<)([:\w]+)/,["delimiter.html",{token:"tag.html",next:"@otherTag"}]],[/(<\/)([\w\-]+)/,["delimiter.html",{token:"tag.html",next:"@otherTag"}]],[/</,"delimiter.html"],[/\{/,"delimiter.html"],[/[^<{]+/]],comment:[[/\{\%\s*endcomment\s*\%\}/,"comment.end.liquid","@pop"],[/./,"comment.content.liquid"]],otherTag:[[/\{\{/,{token:"@rematch",switchTo:"@liquidState.otherTag"}],[/\{\%/,{token:"@rematch",switchTo:"@liquidState.otherTag"}],[/\/?>/,"delimiter.html","@pop"],[/"([^"]*)"/,"attribute.value"],[/'([^']*)'/,"attribute.value"],[/[\w\-]+/,"attribute.name"],[/=/,"delimiter"],[/[ \t\r\n]+/]],liquidState:[[/\{\{/,"delimiter.output.liquid"],[/\}\}/,{token:"delimiter.output.liquid",switchTo:"@$S2.$S3"}],[/\{\%/,"delimiter.tag.liquid"],[/raw\s*\%\}/,"delimiter.tag.liquid","@liquidRaw"],[/\%\}/,{token:"delimiter.tag.liquid",switchTo:"@$S2.$S3"}],{include:"liquidRoot"}],liquidRaw:[[/^(?!\{\%\s*endraw\s*\%\}).+/],[/\{\%/,"delimiter.tag.liquid"],[/@identifier/],[/\%\}/,{token:"delimiter.tag.liquid",next:"@root"}]],liquidRoot:[[/\d+(\.\d+)?/,"number.liquid"],[/"[^"]*"/,"string.liquid"],[/'[^']*'/,"string.liquid"],[/\s+/],[/@symbol/,{cases:{"@operators":"operator.liquid","@default":""}}],[/\./],[/@identifier/,{cases:{"@constants":"keyword.liquid","@builtinFilters":"predefined.liquid","@builtinTags":"predefined.liquid","@default":"variable.liquid"}}],[/[^}|%]/,"variable.liquid"]]}};return p(w)})()));