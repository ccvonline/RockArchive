-- JE: Change system email for Group Leader Pending Notifications
UPDATE [SystemEmail]
SET [Subject] = 'New Pending Group Members | {{ ''Global''  | Attribute:''OrganizationName'' }}'
	, [Body] = '{{ ''Global'' | Attribute:''EmailHeader'' }}

<p>
    {{ Person.NickName }},
</p>

<p>
    We wanted to make you aware of additional individuals who have taken the next step to connect with 
    group. The individuals'' names and contact information can be found below. Our 
    goal is to contact new members within 24-48 hours of receiving this e-mail.
</p>

<table cellpadding="25">
{% for pendingIndividual in PendingIndividuals %}
    <tr><td>
        <strong>{{ pendingIndividual.FullName }}</strong><br />
        {% assign mobilePhone = pendingIndividual.PhoneNumbers | Where:''NumberTypeValueId'', 136 | Select:''NumberFormatted'' %}
        {% assign homePhone = pendingIndividual.PhoneNumbers | Where:''NumberTypeValueId'', 13 | Select:''NumberFormatted'' %}
        {% assign homeAddress = pendingIndividual | Address:''Home'' %}
        
        {% if mobilePhone != empty %}
            Mobile Phone: {{ mobilePhone }}<br />
        {% endif %}
        
        {% if homePhone != empty %}
            Home Phone: {{ homePhone }}<br />
        {% endif %}
        
        {% if pendingIndividual.Email != empty %}
            {{ pendingIndividual.Email }}<br />
        {% endif %}
        
        <p>
        {% if homeAddress != empty %}
            Home Address <br />
            {{ homeAddress }}
        {% endif %}
        </p>
        
    </td></tr>
{% endfor %}
</table>


<p>
    Once you have connected with these individuals, please mark them as active.
</p>

<p>
    Thank you for your on-going commitment to {{ ''Global'' | Attribute:''OrganizationName'' }}.
</p>

{{ ''Global'' | Attribute:''EmailFooter'' }}'
WHERE [Guid] = '18521B26-1C7D-E287-487D-97D176CA4986'

-- TC: Previous Church attribute description change (Issue #939)
UPDATE [Attribute] SET [Description] = N'The church or denomination that this person attended prior to visiting here'
WHERE [GUID] = '5212E7F5-41A1-41F2-80FE-5D971258566D'

-- MP: Missing Communication DateTime Indexes
IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_CreatedDateTime' AND object_id = OBJECT_ID('Communication'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_CreatedDateTime] ON [dbo].[Communication]
    (
        [CreatedDateTime] ASC
    )
END

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_FutureSendDateTime' AND object_id = OBJECT_ID('Communication'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_FutureSendDateTime] ON [dbo].[Communication]
    (
        [FutureSendDateTime] ASC
    )
END

-- JE: Calendar Lava Updates
UPDATE AV SET [Value] = '{% include ''~~/Assets/Lava/CalendarItem.lava'' %}'
FROM [AttributeValue] AV
INNER JOIN [Attribute] A ON A.[Id] = AV.[AttributeId]
INNER JOIN [Block] B ON B.[Id] = AV.[EntityId]
WHERE A.[Guid] = '100F84DD-4526-41CD-80BC-246E15CC8E04'
AND B.[Guid] = 'FC400B7B-760A-4090-9E9E-F049631E7BB4'

UPDATE AV SET [Value] = '{% include ''~~/Assets/Lava/Calendar.lava'' %}'
FROM [AttributeValue] AV
INNER JOIN [Attribute] A ON A.[Id] = AV.[AttributeId]
INNER JOIN [Block] B ON B.[Id] = AV.[EntityId]
WHERE A.[Guid] = '1D3EC083-581E-4435-8FC8-930C48AC50F4'
AND B.[Guid] = '0ADEEFE5-8293-48AC-AFA9-E0F0E363FCE7'

-- JE: Change Involvement Icon
UPDATE [ConnectionType] SET [IconCssClass] = 'fa fa-leaf'
WHERE [Guid] = 'dd565087-a4be-4943-b123-bf22777e8426'

-- JE: Icons for Connection Pages
UPDATE [Page] SET [IconCssClass] = 'fa fa-plug' WHERE [Guid] = '530860ED-BC73-4A43-8E7C-69533EF2B6AD'
UPDATE [Page] SET [IconCssClass] = 'fa fa-plug' WHERE [Guid] = '50F04E77-8D3B-4268-80AB-BC15DD6CB262'