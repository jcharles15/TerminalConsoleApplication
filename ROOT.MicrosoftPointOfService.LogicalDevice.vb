Imports System
Imports System.ComponentModel
Imports System.Management
Imports System.Collections
Imports System.Globalization
Imports System.ComponentModel.Design.Serialization
Imports System.Reflection

Namespace ROOT.MICROSOFTPOINTOFSERVICE
    
    'Functions ShouldSerialize<PropertyName> are functions used by VS property browser to check if a particular property has to be serialized. These functions are added for all ValueType properties ( properties of type Int32, BOOL etc.. which cannot be set to null). These functions use Is<PropertyName>Null function. These functions are also used in the TypeConverter implementation for the properties to check for NULL value of property so that an empty value can be shown in Property browser in case of Drag and Drop in Visual studio.
    'Functions Is<PropertyName>Null() are used to check if a property is NULL.
    'Functions Reset<PropertyName> are added for Nullable Read/Write properties. These functions are used by VS designer in property browser to set a property to NULL.
    'Every property added to the class for WMI property has attributes set to define its behavior in Visual Studio designer and also to define a TypeConverter to be used.
    'An Early Bound class generated for the WMI class.LogicalDevice
    Public Class LogicalDevice
        Inherits System.ComponentModel.Component
        
        'Private property to hold the WMI namespace in which the class resides.
        Private Shared CreatedWmiNamespace As String = "ROOT\MicrosoftPointOfService"
        
        'Private property to hold the name of WMI class which created this class.
        Private Shared CreatedClassName As String = "LogicalDevice"
        
        'Private member variable to hold the ManagementScope which is used by the various methods.
        Private Shared statMgmtScope As System.Management.ManagementScope = Nothing
        
        Private PrivateSystemProperties As ManagementSystemProperties
        
        'Underlying lateBound WMI object.
        Private PrivateLateBoundObject As System.Management.ManagementObject
        
        'Member variable to store the 'automatic commit' behavior for the class.
        Private AutoCommitProp As Boolean
        
        'Private variable to hold the embedded property representing the instance.
        Private embeddedObj As System.Management.ManagementBaseObject
        
        'The current WMI object used
        Private curObj As System.Management.ManagementBaseObject
        
        'Flag to indicate if the instance is an embedded object.
        Private isEmbedded As Boolean
        
        'Below are different overloads of constructors to initialize an instance of the class with a WMI object.
        Public Sub New()
            MyBase.New
            Me.InitializeObject(Nothing, Nothing, Nothing)
        End Sub
        
        Public Sub New(ByVal keyPath As String)
            MyBase.New
            Me.InitializeObject(Nothing, New System.Management.ManagementPath(LogicalDevice.ConstructPath(keyPath)), Nothing)
        End Sub
        
        Public Sub New(ByVal mgmtScope As System.Management.ManagementScope, ByVal keyPath As String)
            MyBase.New
            Me.InitializeObject(CType(mgmtScope,System.Management.ManagementScope), New System.Management.ManagementPath(LogicalDevice.ConstructPath(keyPath)), Nothing)
        End Sub
        
        Public Sub New(ByVal path As System.Management.ManagementPath, ByVal getOptions As System.Management.ObjectGetOptions)
            MyBase.New
            Me.InitializeObject(Nothing, path, getOptions)
        End Sub
        
        Public Sub New(ByVal mgmtScope As System.Management.ManagementScope, ByVal path As System.Management.ManagementPath)
            MyBase.New
            Me.InitializeObject(mgmtScope, path, Nothing)
        End Sub
        
        Public Sub New(ByVal path As System.Management.ManagementPath)
            MyBase.New
            Me.InitializeObject(Nothing, path, Nothing)
        End Sub
        
        Public Sub New(ByVal mgmtScope As System.Management.ManagementScope, ByVal path As System.Management.ManagementPath, ByVal getOptions As System.Management.ObjectGetOptions)
            MyBase.New
            Me.InitializeObject(mgmtScope, path, getOptions)
        End Sub
        
        Public Sub New(ByVal theObject As System.Management.ManagementObject)
            MyBase.New
            Initialize
            If (CheckIfProperClass(theObject) = true) Then
                PrivateLateBoundObject = theObject
                PrivateSystemProperties = New ManagementSystemProperties(PrivateLateBoundObject)
                curObj = PrivateLateBoundObject
            Else
                Throw New System.ArgumentException("Class name does not match.")
            End If
        End Sub
        
        Public Sub New(ByVal theObject As System.Management.ManagementBaseObject)
            MyBase.New
            Initialize
            If (CheckIfProperClass(theObject) = true) Then
                embeddedObj = theObject
                PrivateSystemProperties = New ManagementSystemProperties(theObject)
                curObj = embeddedObj
                isEmbedded = true
            Else
                Throw New System.ArgumentException("Class name does not match.")
            End If
        End Sub
        
        'Property returns the namespace of the WMI class.
        <Browsable(true),  _
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>  _
        Public ReadOnly Property OriginatingNamespace() As String
            Get
                Return "ROOT\MicrosoftPointOfService"
            End Get
        End Property
        
        <Browsable(true),  _
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>  _
        Public ReadOnly Property ManagementClassName() As String
            Get
                Dim strRet As String = CreatedClassName
                If (Not (curObj) Is Nothing) Then
                    If (Not (curObj.ClassPath) Is Nothing) Then
                        strRet = CType(curObj("__CLASS"),String)
                        If ((strRet Is Nothing)  _
                                    OrElse (strRet Is String.Empty)) Then
                            strRet = CreatedClassName
                        End If
                    End If
                End If
                Return strRet
            End Get
        End Property
        
        'Property pointing to an embedded object to get System properties of the WMI object.
        <Browsable(true),  _
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>  _
        Public ReadOnly Property SystemProperties() As ManagementSystemProperties
            Get
                Return PrivateSystemProperties
            End Get
        End Property
        
        'Property returning the underlying lateBound object.
        <Browsable(false),  _
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>  _
        Public ReadOnly Property LateBoundObject() As System.Management.ManagementBaseObject
            Get
                Return curObj
            End Get
        End Property
        
        'ManagementScope of the object.
        <Browsable(true),  _
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>  _
        Public Property Scope() As System.Management.ManagementScope
            Get
                If (isEmbedded = false) Then
                    Return PrivateLateBoundObject.Scope
                Else
                    Return Nothing
                End If
            End Get
            Set
                If (isEmbedded = false) Then
                    PrivateLateBoundObject.Scope = value
                End If
            End Set
        End Property
        
        'Property to show the commit behavior for the WMI object. If true, WMI object will be automatically saved after each property modification.(ie. Put() is called after modification of a property).
        <Browsable(false),  _
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>  _
        Public Property AutoCommit() As Boolean
            Get
                Return AutoCommitProp
            End Get
            Set
                AutoCommitProp = value
            End Set
        End Property
        
        'The ManagementPath of the underlying WMI object.
        <Browsable(true)>  _
        Public Property Path() As System.Management.ManagementPath
            Get
                If (isEmbedded = false) Then
                    Return PrivateLateBoundObject.Path
                Else
                    Return Nothing
                End If
            End Get
            Set
                If (isEmbedded = false) Then
                    If (CheckIfProperClass(Nothing, value, Nothing) <> true) Then
                        Throw New System.ArgumentException("Class name does not match.")
                    End If
                    PrivateLateBoundObject.Path = value
                End If
            End Set
        End Property
        
        'Public static scope property which is used by the various methods.
        <Browsable(true),  _
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>  _
        Public Shared Property StaticScope() As System.Management.ManagementScope
            Get
                Return statMgmtScope
            End Get
            Set
                statMgmtScope = value
            End Set
        End Property
        
        <Browsable(true),  _
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>  _
        Public ReadOnly Property Name() As String
            Get
                Return CType(curObj("Name"),String)
            End Get
        End Property
        
        <Browsable(true),  _
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>  _
        Public ReadOnly Property Path0() As String
            Get
                Return CType(curObj("Path"),String)
            End Get
        End Property
        
        <Browsable(true),  _
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>  _
        Public ReadOnly Property SoName() As String
            Get
                Return CType(curObj("SoName"),String)
            End Get
        End Property
        
        <Browsable(true),  _
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>  _
        Public ReadOnly Property Type() As String
            Get
                Return CType(curObj("Type"),String)
            End Get
        End Property
        
        Private Overloads Function CheckIfProperClass(ByVal mgmtScope As System.Management.ManagementScope, ByVal path As System.Management.ManagementPath, ByVal OptionsParam As System.Management.ObjectGetOptions) As Boolean
            If ((Not (path) Is Nothing)  _
                        AndAlso (String.Compare(path.ClassName, Me.ManagementClassName, true, System.Globalization.CultureInfo.InvariantCulture) = 0)) Then
                Return true
            Else
                Return CheckIfProperClass(New System.Management.ManagementObject(mgmtScope, path, OptionsParam))
            End If
        End Function
        
        Private Overloads Function CheckIfProperClass(ByVal theObj As System.Management.ManagementBaseObject) As Boolean
            If ((Not (theObj) Is Nothing)  _
                        AndAlso (String.Compare(CType(theObj("__CLASS"),String), Me.ManagementClassName, true, System.Globalization.CultureInfo.InvariantCulture) = 0)) Then
                Return true
            Else
                Dim parentClasses As System.Array = CType(theObj("__DERIVATION"),System.Array)
                If (Not (parentClasses) Is Nothing) Then
                    Dim count As Integer = 0
                    count = 0
                    Do While (count < parentClasses.Length)
                        If (String.Compare(CType(parentClasses.GetValue(count),String), Me.ManagementClassName, true, System.Globalization.CultureInfo.InvariantCulture) = 0) Then
                            Return true
                        End If
                        count = (count + 1)
                    Loop
                End If
            End If
            Return false
        End Function
        
        <Browsable(true)>  _
        Public Overloads Sub CommitObject()
            If (isEmbedded = false) Then
                PrivateLateBoundObject.Put
            End If
        End Sub
        
        <Browsable(true)>  _
        Public Overloads Sub CommitObject(ByVal putOptions As System.Management.PutOptions)
            If (isEmbedded = false) Then
                PrivateLateBoundObject.Put(putOptions)
            End If
        End Sub
        
        Private Sub Initialize()
            AutoCommitProp = true
            isEmbedded = false
        End Sub
        
        Private Shared Function ConstructPath(ByVal keyPath As String) As String
            Dim strPath As String = "ROOT\MicrosoftPointOfService:LogicalDevice"
            strPath = String.Concat(strPath, String.Concat(".Path=", String.Concat("""", String.Concat(keyPath, """"))))
            Return strPath
        End Function
        
        Private Sub InitializeObject(ByVal mgmtScope As System.Management.ManagementScope, ByVal path As System.Management.ManagementPath, ByVal getOptions As System.Management.ObjectGetOptions)
            Initialize
            If (Not (path) Is Nothing) Then
                If (CheckIfProperClass(mgmtScope, path, getOptions) <> true) Then
                    Throw New System.ArgumentException("Class name does not match.")
                End If
            End If
            PrivateLateBoundObject = New System.Management.ManagementObject(mgmtScope, path, getOptions)
            PrivateSystemProperties = New ManagementSystemProperties(PrivateLateBoundObject)
            curObj = PrivateLateBoundObject
        End Sub
        
        'Different overloads of GetInstances() help in enumerating instances of the WMI class.
        Public Overloads Shared Function GetInstances() As LogicalDeviceCollection
            Return GetInstances(Nothing, Nothing, Nothing)
        End Function
        
        Public Overloads Shared Function GetInstances(ByVal condition As String) As LogicalDeviceCollection
            Return GetInstances(Nothing, condition, Nothing)
        End Function
        
        Public Overloads Shared Function GetInstances(ByVal selectedProperties() As System.String ) As LogicalDeviceCollection
            Return GetInstances(Nothing, Nothing, selectedProperties)
        End Function
        
        Public Overloads Shared Function GetInstances(ByVal condition As String, ByVal selectedProperties() As System.String ) As LogicalDeviceCollection
            Return GetInstances(Nothing, condition, selectedProperties)
        End Function
        
        Public Overloads Shared Function GetInstances(ByVal mgmtScope As System.Management.ManagementScope, ByVal enumOptions As System.Management.EnumerationOptions) As LogicalDeviceCollection
            If (mgmtScope Is Nothing) Then
                If (statMgmtScope Is Nothing) Then
                    mgmtScope = New System.Management.ManagementScope()
                    mgmtScope.Path.NamespacePath = "root\MicrosoftPointOfService"
                Else
                    mgmtScope = statMgmtScope
                End If
            End If
            Dim pathObj As System.Management.ManagementPath = New System.Management.ManagementPath()
            pathObj.ClassName = "LogicalDevice"
            pathObj.NamespacePath = "root\MicrosoftPointOfService"
            Dim clsObject As System.Management.ManagementClass = New System.Management.ManagementClass(mgmtScope, pathObj, Nothing)
            If (enumOptions Is Nothing) Then
                enumOptions = New System.Management.EnumerationOptions()
                enumOptions.EnsureLocatable = true
            End If
            Return New LogicalDeviceCollection(clsObject.GetInstances(enumOptions))
        End Function
        
        Public Overloads Shared Function GetInstances(ByVal mgmtScope As System.Management.ManagementScope, ByVal condition As String) As LogicalDeviceCollection
            Return GetInstances(mgmtScope, condition, Nothing)
        End Function
        
        Public Overloads Shared Function GetInstances(ByVal mgmtScope As System.Management.ManagementScope, ByVal selectedProperties() As System.String ) As LogicalDeviceCollection
            Return GetInstances(mgmtScope, Nothing, selectedProperties)
        End Function
        
        Public Overloads Shared Function GetInstances(ByVal mgmtScope As System.Management.ManagementScope, ByVal condition As String, ByVal selectedProperties() As System.String ) As LogicalDeviceCollection
            If (mgmtScope Is Nothing) Then
                If (statMgmtScope Is Nothing) Then
                    mgmtScope = New System.Management.ManagementScope()
                    mgmtScope.Path.NamespacePath = "root\MicrosoftPointOfService"
                Else
                    mgmtScope = statMgmtScope
                End If
            End If
            Dim ObjectSearcher As System.Management.ManagementObjectSearcher = New System.Management.ManagementObjectSearcher(mgmtScope, New SelectQuery("LogicalDevice", condition, selectedProperties))
            Dim enumOptions As System.Management.EnumerationOptions = New System.Management.EnumerationOptions()
            enumOptions.EnsureLocatable = true
            ObjectSearcher.Options = enumOptions
            Return New LogicalDeviceCollection(ObjectSearcher.Get)
        End Function
        
        <Browsable(true)>  _
        Public Shared Function CreateInstance() As LogicalDevice
            Dim mgmtScope As System.Management.ManagementScope = Nothing
            If (statMgmtScope Is Nothing) Then
                mgmtScope = New System.Management.ManagementScope()
                mgmtScope.Path.NamespacePath = CreatedWmiNamespace
            Else
                mgmtScope = statMgmtScope
            End If
            Dim mgmtPath As System.Management.ManagementPath = New System.Management.ManagementPath(CreatedClassName)
            Dim tmpMgmtClass As System.Management.ManagementClass = New System.Management.ManagementClass(mgmtScope, mgmtPath, Nothing)
            Return New LogicalDevice(tmpMgmtClass.CreateInstance)
        End Function
        
        <Browsable(true)>  _
        Public Sub Delete()
            PrivateLateBoundObject.Delete
        End Sub
        
        'Enumerator implementation for enumerating instances of the class.
        Public Class LogicalDeviceCollection
            Inherits Object
            Implements ICollection
            
            Private privColObj As ManagementObjectCollection
            
            Public Sub New(ByVal objCollection As ManagementObjectCollection)
                MyBase.New
                privColObj = objCollection
            End Sub
            
            Public Overridable ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
                Get
                    Return privColObj.Count
                End Get
            End Property
            
            Public Overridable ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
                Get
                    Return privColObj.IsSynchronized
                End Get
            End Property
            
            Public Overridable ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
                Get
                    Return Me
                End Get
            End Property
            
            Public Overridable Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
                privColObj.CopyTo(array, index)
                Dim nCtr As Integer
                nCtr = 0
                Do While (nCtr < array.Length)
                    array.SetValue(New LogicalDevice(CType(array.GetValue(nCtr),System.Management.ManagementObject)), nCtr)
                    nCtr = (nCtr + 1)
                Loop
            End Sub
            
            Public Overridable Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
                Return New LogicalDeviceEnumerator(privColObj.GetEnumerator)
            End Function
            
            Public Class LogicalDeviceEnumerator
                Inherits Object
                Implements System.Collections.IEnumerator
                
                Private privObjEnum As ManagementObjectCollection.ManagementObjectEnumerator
                
                Public Sub New(ByVal objEnum As ManagementObjectCollection.ManagementObjectEnumerator)
                    MyBase.New
                    privObjEnum = objEnum
                End Sub
                
                Public Overridable ReadOnly Property Current() As Object Implements System.Collections.IEnumerator.Current
                    Get
                        Return New LogicalDevice(CType(privObjEnum.Current,System.Management.ManagementObject))
                    End Get
                End Property
                
                Public Overridable Function MoveNext() As Boolean Implements System.Collections.IEnumerator.MoveNext
                    Return privObjEnum.MoveNext
                End Function
                
                Public Overridable Sub Reset() Implements System.Collections.IEnumerator.Reset
                    privObjEnum.Reset
                End Sub
            End Class
        End Class
        
        'TypeConverter to handle null values for ValueType properties
        Public Class WMIValueTypeConverter
            Inherits TypeConverter
            
            Private baseConverter As TypeConverter
            
            Private baseType As System.Type
            
            Public Sub New(ByVal inBaseType As System.Type)
                MyBase.New
                baseConverter = TypeDescriptor.GetConverter(inBaseType)
                baseType = inBaseType
            End Sub
            
            Public Overloads Overrides Function CanConvertFrom(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal srcType As System.Type) As Boolean
                Return baseConverter.CanConvertFrom(context, srcType)
            End Function
            
            Public Overloads Overrides Function CanConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal destinationType As System.Type) As Boolean
                Return baseConverter.CanConvertTo(context, destinationType)
            End Function
            
            Public Overloads Overrides Function ConvertFrom(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object) As Object
                Return baseConverter.ConvertFrom(context, culture, value)
            End Function
            
            Public Overloads Overrides Function CreateInstance(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal dictionary As System.Collections.IDictionary) As Object
                Return baseConverter.CreateInstance(context, dictionary)
            End Function
            
            Public Overloads Overrides Function GetCreateInstanceSupported(ByVal context As System.ComponentModel.ITypeDescriptorContext) As Boolean
                Return baseConverter.GetCreateInstanceSupported(context)
            End Function
            
            Public Overloads Overrides Function GetProperties(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal value As Object, ByVal attributeVar() As System.Attribute) As PropertyDescriptorCollection
                Return baseConverter.GetProperties(context, value, attributeVar)
            End Function
            
            Public Overloads Overrides Function GetPropertiesSupported(ByVal context As System.ComponentModel.ITypeDescriptorContext) As Boolean
                Return baseConverter.GetPropertiesSupported(context)
            End Function
            
            Public Overloads Overrides Function GetStandardValues(ByVal context As System.ComponentModel.ITypeDescriptorContext) As System.ComponentModel.TypeConverter.StandardValuesCollection
                Return baseConverter.GetStandardValues(context)
            End Function
            
            Public Overloads Overrides Function GetStandardValuesExclusive(ByVal context As System.ComponentModel.ITypeDescriptorContext) As Boolean
                Return baseConverter.GetStandardValuesExclusive(context)
            End Function
            
            Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As System.ComponentModel.ITypeDescriptorContext) As Boolean
                Return baseConverter.GetStandardValuesSupported(context)
            End Function
            
            Public Overloads Overrides Function ConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object, ByVal destinationType As System.Type) As Object
                If (baseType.BaseType Is GetType(System.[Enum])) Then
                    If (value.GetType Is destinationType) Then
                        Return value
                    End If
                    If (((value Is Nothing)  _
                                AndAlso (Not (context) Is Nothing))  _
                                AndAlso (context.PropertyDescriptor.ShouldSerializeValue(context.Instance) = false)) Then
                        Return  "NULL_ENUM_VALUE" 
                    End If
                    Return baseConverter.ConvertTo(context, culture, value, destinationType)
                End If
                If ((baseType Is GetType(Boolean))  _
                            AndAlso (baseType.BaseType Is GetType(System.ValueType))) Then
                    If (((value Is Nothing)  _
                                AndAlso (Not (context) Is Nothing))  _
                                AndAlso (context.PropertyDescriptor.ShouldSerializeValue(context.Instance) = false)) Then
                        Return ""
                    End If
                    Return baseConverter.ConvertTo(context, culture, value, destinationType)
                End If
                If ((Not (context) Is Nothing)  _
                            AndAlso (context.PropertyDescriptor.ShouldSerializeValue(context.Instance) = false)) Then
                    Return ""
                End If
                Return baseConverter.ConvertTo(context, culture, value, destinationType)
            End Function
        End Class
        
        'Embedded class to represent WMI system Properties.
        <TypeConverter(GetType(System.ComponentModel.ExpandableObjectConverter))>  _
        Public Class ManagementSystemProperties
            
            Private PrivateLateBoundObject As System.Management.ManagementBaseObject
            
            Public Sub New(ByVal ManagedObject As System.Management.ManagementBaseObject)
                MyBase.New
                PrivateLateBoundObject = ManagedObject
            End Sub
            
            <Browsable(true)>  _
            Public ReadOnly Property GENUS() As Integer
                Get
                    Return CType(PrivateLateBoundObject("__GENUS"),Integer)
                End Get
            End Property
            
            <Browsable(true)>  _
            Public ReadOnly Property [CLASS]() As String
                Get
                    Return CType(PrivateLateBoundObject("__CLASS"),String)
                End Get
            End Property
            
            <Browsable(true)>  _
            Public ReadOnly Property SUPERCLASS() As String
                Get
                    Return CType(PrivateLateBoundObject("__SUPERCLASS"),String)
                End Get
            End Property
            
            <Browsable(true)>  _
            Public ReadOnly Property DYNASTY() As String
                Get
                    Return CType(PrivateLateBoundObject("__DYNASTY"),String)
                End Get
            End Property
            
            <Browsable(true)>  _
            Public ReadOnly Property RELPATH() As String
                Get
                    Return CType(PrivateLateBoundObject("__RELPATH"),String)
                End Get
            End Property
            
            <Browsable(true)>  _
            Public ReadOnly Property PROPERTY_COUNT() As Integer
                Get
                    Return CType(PrivateLateBoundObject("__PROPERTY_COUNT"),Integer)
                End Get
            End Property
            
            <Browsable(true)>  _
            Public ReadOnly Property DERIVATION() As String()
                Get
                    Return CType(PrivateLateBoundObject("__DERIVATION"),String())
                End Get
            End Property
            
            <Browsable(true)>  _
            Public ReadOnly Property SERVER() As String
                Get
                    Return CType(PrivateLateBoundObject("__SERVER"),String)
                End Get
            End Property
            
            <Browsable(true)>  _
            Public ReadOnly Property [NAMESPACE]() As String
                Get
                    Return CType(PrivateLateBoundObject("__NAMESPACE"),String)
                End Get
            End Property
            
            <Browsable(true)>  _
            Public ReadOnly Property PATH() As String
                Get
                    Return CType(PrivateLateBoundObject("__PATH"),String)
                End Get
            End Property
        End Class
    End Class
End Namespace
