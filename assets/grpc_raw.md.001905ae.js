import{_ as s,c as a,o as n,b as l}from"./app.40687699.js";const i=JSON.parse('{"title":"Low-level access to RPC Call","description":"","frontmatter":{},"headers":[],"relativePath":"grpc/raw.md","lastUpdated":1673148208000}'),p={name:"grpc/raw.md"},o=l(`<h1 id="low-level-access-to-rpc-call" tabindex="-1">Low-level access to RPC Call <a class="header-anchor" href="#low-level-access-to-rpc-call" aria-hidden="true">#</a></h1><p>You may not have enough ready-made functions for testing in FlueFlame. In this case, you can directly access objects such as <code>IClientStreamWriter</code> and <code>IAsyncStreamReader</code>:</p><div class="language-csharp"><button title="Copy Code" class="copy"></button><span class="lang">csharp</span><pre class="shiki"><code><span class="line"><span style="color:#A6ACCD;">GrpcHost</span></span>
<span class="line"><span style="color:#A6ACCD;">	</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">CreateClient</span><span style="color:#89DDFF;">&lt;</span><span style="color:#FFCB6B;">EmployeeService</span><span style="color:#89DDFF;">.</span><span style="color:#FFCB6B;">EmployeeServiceClient</span><span style="color:#89DDFF;">&gt;()</span></span>
<span class="line"><span style="color:#A6ACCD;">	</span><span style="color:#89DDFF;">.</span><span style="color:#A6ACCD;">Bidirectional</span></span>
<span class="line"><span style="color:#A6ACCD;">		</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">Call</span><span style="color:#89DDFF;">(</span><span style="color:#FFCB6B;">c</span><span style="color:#89DDFF;">=&gt;</span><span style="color:#A6ACCD;">c</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">GetByIds</span><span style="color:#89DDFF;">())</span></span>
<span class="line"><span style="color:#A6ACCD;">		</span><span style="color:#89DDFF;">.</span><span style="color:#A6ACCD;">RequestStream</span></span>
<span class="line"><span style="color:#A6ACCD;">			</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">WithStreamWriter</span><span style="color:#89DDFF;">(</span><span style="color:#C792EA;">async</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">writer</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">=&gt;</span></span>
<span class="line"><span style="color:#A6ACCD;">			</span><span style="color:#89DDFF;">{</span></span>
<span class="line"><span style="color:#A6ACCD;">				</span><span style="color:#F78C6C;">await</span><span style="color:#A6ACCD;"> writer</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">WriteAsync</span><span style="color:#89DDFF;">(</span><span style="color:#F78C6C;">new</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">StringValue</span><span style="color:#89DDFF;">()</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">{</span><span style="color:#A6ACCD;"> Value </span><span style="color:#89DDFF;">=</span><span style="color:#A6ACCD;"> Guid</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">NewGuid</span><span style="color:#89DDFF;">().</span><span style="color:#82AAFF;">ToString</span><span style="color:#89DDFF;">()});</span></span>
<span class="line"><span style="color:#A6ACCD;">				</span><span style="color:#F78C6C;">await</span><span style="color:#A6ACCD;"> writer</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">CompleteAsync</span><span style="color:#89DDFF;">();</span></span>
<span class="line"><span style="color:#A6ACCD;">			</span><span style="color:#89DDFF;">})</span></span>
<span class="line"><span style="color:#A6ACCD;">		</span><span style="color:#89DDFF;">.</span><span style="color:#A6ACCD;">ResponseStream</span></span>
<span class="line"><span style="color:#A6ACCD;">			</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">WithStreamReader</span><span style="color:#89DDFF;">(</span><span style="color:#C792EA;">async</span><span style="color:#A6ACCD;"> </span><span style="color:#FFCB6B;">reader</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">=&gt;</span></span>
<span class="line"><span style="color:#A6ACCD;">			</span><span style="color:#89DDFF;">{</span></span>
<span class="line"><span style="color:#A6ACCD;">				</span><span style="color:#F78C6C;">await</span><span style="color:#A6ACCD;"> FluentActions</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">Awaiting</span><span style="color:#89DDFF;">(</span><span style="color:#C792EA;">async</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">()</span><span style="color:#A6ACCD;"> </span><span style="color:#89DDFF;">=&gt;</span><span style="color:#A6ACCD;"> </span><span style="color:#F78C6C;">await</span><span style="color:#A6ACCD;"> reader</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">MoveNext</span><span style="color:#89DDFF;">())</span></span>
<span class="line"><span style="color:#A6ACCD;">					</span><span style="color:#89DDFF;">.</span><span style="color:#82AAFF;">Should</span><span style="color:#89DDFF;">().</span><span style="color:#82AAFF;">ThrowAsync</span><span style="color:#89DDFF;">&lt;</span><span style="color:#FFCB6B;">RpcException</span><span style="color:#89DDFF;">&gt;();</span></span>
<span class="line"><span style="color:#A6ACCD;">			</span><span style="color:#89DDFF;">});</span></span>
<span class="line"></span></code></pre></div>`,3),e=[o];function t(c,r,F,y,D,A){return n(),a("div",null,e)}const d=s(p,[["render",t]]);export{i as __pageData,d as default};
