using UnityEngine;
using UnityEditor;

public class StandardMaterialGUI : ShaderGUI
{
    private static class Styles
    {
        // Setup Bools - Maybe thse can be toggle buttons?
        public static GUIContent hasDetailNormalLabel = EditorGUIUtility.TrTextContent("Base Color", "Base Color for the material");
        public static GUIContent hasHeightMapLabel = EditorGUIUtility.TrTextContent("Base Color", "Base Color for the material");

        // Base Color Section
        public static GUIContent baseColorLabel = EditorGUIUtility.TrTextContent("Base Color", "Base Color for the material");

        // Normal, Height, and Detial Normal Section
        public static GUIContent normalsLabel = EditorGUIUtility.TrTextContent("Normals", "2D Texture map for the Normals");
        public static GUIContent normalStrengthLabel = EditorGUIUtility.TrTextContent("Normal Strength", "Strength of the normal");
        public static GUIContent heightMultLabel = EditorGUIUtility.TrTextContent("Height Multiplier", "Multiplier for height map as parallax");
        public static GUIContent detailNormalLabel = EditorGUIUtility.TrTextContent("Detail Normal", "2D Texture for Detail Normal Map");
        public static GUIContent detailNormalScaleLabel = EditorGUIUtility.TrTextContent("Detail Normal Scale", "Detail Normal Scale of UVs");
        public static GUIContent detailNormalStrengthLabel = EditorGUIUtility.TrTextContent("Detail Normal Strength", "Detail normal strength");

        // MOES (Metallic, Ambient Occlusion, Emissive, Speculative (Glossiness) section
        public static GUIContent moesMapLabel = EditorGUIUtility.TrTextContent("MOES Map", "Metallic, Ambient Occlusion, Emissive, Speculative (Glossiness)");
        public static GUIContent emisTintLabel = EditorGUIUtility.TrTextContent("Emissive Tint", "Tint across entire emissive");
        public static GUIContent emisBoostLabel = EditorGUIUtility.TrTextContent("Emissive Boost", "BOOST THE EMISSIVE!");
        public static GUIContent glossLabel = EditorGUIUtility.TrTextContent("Glossiness modifier", "Gloss amount, default to .3");
        public static GUIContent metallicLabel = EditorGUIUtility.TrTextContent("Metallic modifier", "Matallic amount, default to 0");

        public static int labelSpace = 2;
        public static int smallSpace = 5;
        public static int normalSpace = 10;
    }

    MaterialEditor m_MaterialEditor = null;

    MaterialProperty m_hasHeight = null;
    MaterialProperty m_hasDetail = null;

    MaterialProperty m_baseColor = null;

    MaterialProperty m_normals = null;
    MaterialProperty m_normalStrength = null;
    MaterialProperty m_heightMult = null;
    MaterialProperty m_detailNormal = null;
    MaterialProperty m_detailNormalScale = null;
    MaterialProperty m_detailNormalStrength = null;

    MaterialProperty m_moesMap = null;
    MaterialProperty m_emisTint = null;
    MaterialProperty m_emisBoost = null;
    MaterialProperty m_gloss = null;
    MaterialProperty m_metallic = null;

    public void FindProperties(MaterialProperty[] props)
    {
        m_hasHeight = FindProperty("_USEHEIGHT", props);
        m_hasDetail = FindProperty("_DETAILNORMAL", props);

        m_baseColor = FindProperty("_Base_Color", props);

        m_normals = FindProperty("_Normal_Height", props);
        m_normalStrength = FindProperty("_Normal_Strength", props);
        m_heightMult = FindProperty("_Height_Multiplier", props);
        m_detailNormal = FindProperty("_Detail_Normal", props);
        m_detailNormalScale = FindProperty("_Detail_Normal_Scale", props);
        m_detailNormalStrength = FindProperty("_Detail_Normal_Strength", props);

        m_moesMap = FindProperty("_MOES", props);
        m_emisTint = FindProperty("_Emis_Tint", props);
        m_emisBoost = FindProperty("_Emis_Boost", props);
        m_gloss = FindProperty("_Gloss", props);
        m_metallic = FindProperty("_Metallic", props);
    }

    public Rect GetDefaultRect()
    {
        GUIStyle style = new GUIStyle();
        style.padding = new RectOffset(0, 0, 2, 4);
        return GUILayoutUtility.GetRect(GUIContent.none, style);
    }

    public void DrawVector2Field(MaterialProperty prop, GUIContent content)
    {

        EditorGUI.BeginChangeCheck();
        Vector4 vec2Value = EditorGUI.Vector2Field(GetDefaultRect(), content.text, prop.vectorValue);
        GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", content.tooltip));

        if (EditorGUI.EndChangeCheck()) { prop.vectorValue = vec2Value; }
    }

    override public void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        FindProperties(properties);
        m_MaterialEditor = materialEditor;
        Material material = materialEditor.target as Material;
        ShaderPropertiesGUI(material);
    }

    void DoSurfaceField()
    {
        m_MaterialEditor.TexturePropertySingleLine(Styles.baseColorLabel, m_baseColor);
    }

    public void ShaderPropertiesGUI(Material material)
    {
        EditorGUI.indentLevel += 1;


        #region Surface
        if (GUI.Button(new Rect(10, 70, 50, 30), "Click"))
        {
            Debug.Log("Clicked the button with text");
        }
        GUILayout.Space(Styles.labelSpace);
        DoSurfaceField();
        GUILayout.Space(Styles.labelSpace);

        #endregion

        EditorGUI.indentLevel -= 1;

    }
}
