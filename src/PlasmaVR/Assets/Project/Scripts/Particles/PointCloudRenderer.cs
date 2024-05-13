using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.VFX;

public class PointCloudRenderer : Simulation
{
    public string basePath = "";
    public int maxFrame = 0;

    VisualEffect vfx;
    uint resolution = 512;
    uint particleCount = 0;

    public float particleSize = 0.1f;

    [Range(0.0f, 1.0f)]
    public float threshold = 0.0f;

    public bool applyThreshold = false;

    List<Texture2D> positionTextures;


    public override bool startSim(int numFrames, Vector3 dims, string simPath)
    {
        try
        {
            positionTextures = new List<Texture2D>();

            maxFrame = numFrames;
            basePath = simPath + "/Particles";

            loadSimulation();
            vfx = GetComponent<VisualEffect>();
        }
        catch
        {
            return false;
        }
        return true;
    }

    protected override void loadSimulation()
    {
        for (int i = 0; i < maxFrame; i++)
        {
            importAppendPartFile(basePath + "/" + i + ".raw");
        }
    }

    public override void displayFrame(int frame)
    {
        vfx.Reinit();
        vfx.SetUInt(Shader.PropertyToID("ParticleCount"), particleCount);
        vfx.SetTexture(Shader.PropertyToID("TexPosScale"), positionTextures[frame]);
        vfx.SetUInt(Shader.PropertyToID("Resolution"), resolution);
        vfx.SetFloat(Shader.PropertyToID("ParticleSize"), particleSize);
        vfx.SetFloat(Shader.PropertyToID("Threshold"), threshold);
        vfx.SetBool(Shader.PropertyToID("ApplyThreshold"), applyThreshold);
    }

    public void AppendParticles(Vector4[] positions)
    {
        Texture2D texPosScale = new Texture2D(positions.Length > (int)resolution ? (int)resolution : positions.Length, Mathf.Clamp(positions.Length / (int)resolution, 1, (int)resolution), TextureFormat.RGBAFloat, false);

        int texWidth = texPosScale.width;
        int texHeight = texPosScale.height;

        for (int y = 0; y < texHeight; y++)
        {
            for (int x = 0; x < texWidth; x++)
            {
                int index = x + y * texWidth;
                var data = new Color(positions[index].x, positions[index].y, positions[index].z, positions[index].w);
                texPosScale.SetPixel(x, y, data);
            }
        }

        texPosScale.Apply();
        positionTextures.Add(texPosScale);
        particleCount = (uint)positions.Length;
        toUpdate = true;
    }

    private void importAppendPartFile(string path)
    {
        List<Vector4> positions = new List<Vector4>();

        //Test with file, file in format x1, x2, x3, energy
        FileStream file = File.Open(path, FileMode.Open);
        BinaryReader reader = new BinaryReader(file);
        long streamlength = reader.BaseStream.Length;
        while (reader.BaseStream.Position != streamlength)
        {
            Vector4 vec = new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            positions.Add(vec);
        }

        AppendParticles(positions.ToArray());
    }

    public void swapThreshold(float number)
    {
        threshold = number;
    }

    public void toggleThreshold(bool value)
    {
        applyThreshold = value;
    }
}

