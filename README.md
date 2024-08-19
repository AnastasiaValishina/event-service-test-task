# Unity Event Tracking Service

Unity version: 2021.3.41f1

<p>This Unity project contains an event tracking service designed to collect and send analytics events to a server. It is designed to ensure that critical data is not lost, even in the case of application crashes or unexpected shutdowns.</p>

## 🚀 Features

<ul>
    <li><strong>Event Queuing</strong>: Events are queued and sent in batches to minimize server requests. The service employs a cooldown mechanism to gather events over a set period before sending them to the server in one request.</li>
    <li><strong>Guaranteed Delivery</strong>: Events are only removed from the queue after the server successfully processes them (<code>HTTP 200 OK</code>). If the server is unreachable, the events will remain in the queue and be resent when possible.</li>
    <li><strong>Persistent Storage</strong>: Events are stored locally using Unity's <code>Application.persistentDataPath</code>, ensuring that they are retained between sessions. This is especially important for WebGL, where detecting application closure is not always possible.</li>
    <li><strong>Simple Integration</strong>: Easily integrated into any Unity project by attaching the <code>EventService</code> MonoBehaviour to a GameObject.</li>
</ul>

## 🛠️ Usage

<ol>
    <li><strong>Tracking Events</strong>: Use the <code>TrackEvent</code> method to queue events for tracking.</li>
</ol>

<pre><code>
eventService.TrackEvent("levelStart", "level:3");
</code></pre>

<ol start="2">
    <li><strong>Event Types</strong>: Customize the <code>type</code> and <code>data</code> fields to match the specific analytics events you need to track.</li>
    <li><strong>Cooldown Mechanism</strong>: Configure the cooldown period (<code>cooldownBeforeSend</code>) to determine how often events are sent to the server.</li>
</ol>

## 🔧 Setup

<ol>
    <li>Add the <code>EventService</code> script to a GameObject in your scene.</li>
    <li>Configure the <code>serverUrl</code> and other settings directly in the Unity Inspector.</li>
    <li>The service will automatically start tracking and sending events as they occur.</li>
</ol>

