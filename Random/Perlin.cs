namespace TNNUtils.Random
{
    public static class Perlin
    {
        private static readonly int[] Permutation512;
        
        static Perlin()
        {
	        //TODO: Replace this random with custom random
            UnityEngine.Random.InitState(1234);
            
            var permutation256 = new int[256];
            for (var i = 0; i < 256; i++)
            {
                permutation256[i] = UnityEngine.Random.Range(0, 255);
            }
            
            Permutation512 = new int[512];
            for (var i = 0; i < 512; i++)
            {
                Permutation512[i] = permutation256[i % 256];
            }
        }
        
        public static float PerlinNoise(float x)
        {
	        var xi = ((int)(x % 256) + 256) % 256 + 1;
	        
	        var xf = x-(int)x;

	        if (x < 0f)
	        {
		        xf += 1f;
		        xi -= 1;
	        }
            
            var u = Misc.Math.EaseNormal(xf);
            
            var a  = Permutation512[xi];
            var aa = Permutation512[a];
            var b  = Permutation512[xi+1];
            var ba = Permutation512[b];
        
            var x1 = Misc.Math.Lerp(
                Gradient (Permutation512[aa], xf, 0),
                Gradient (Permutation512[ba], xf-1, 0),
                u);

            return (x1+1)/2;	
        }
        
        public static float PerlinNoise(float x, float y)
        { 
	        var xi = ((int)(x % 256) + 256) % 256 + 1;
	        var yi = ((int)(y % 256) + 256) % 256 + 1;
	        
	        var xf = x-(int)x;
	        var yf = y-(int)y;

	        if (x < 0f)
	        {
		        xf += 1f;
		        xi -= 1;
	        }

	        if (y < 0f)
	        {
		        yf += 1f;
		        yi -= 1;
	        }
	        
	        var u = Misc.Math.EaseNormal(xf);
	        var v = Misc.Math.EaseNormal(yf);
			
	        var a  = Permutation512[xi]+yi;
	        var aa = Permutation512[a];
	        var ab = Permutation512[a+1];
	        var b  = Permutation512[xi+1]+yi;
	        var ba = Permutation512[b];
	        var bb = Permutation512[b+1];
			
	        var x1 = Misc.Math.Lerp(	
		        Gradient(Permutation512[aa], xf, yf),			
		        Gradient(Permutation512[ba], xf-1, yf),			
		        u);										
	        var x2 = Misc.Math.Lerp(	
		        Gradient(Permutation512[ab], xf, yf-1),
		        Gradient(Permutation512[bb], xf-1, yf-1),
		        u);
	        var y1 = Misc.Math.Lerp(x1, x2, v);

	        return (y1+1)/2;
        }
        
        public static float PerlinNoise(float x, float y, float z)
        { 
	        var xi = ((int)(x % 256) + 256) % 256 + 1;
	        var yi = ((int)(y % 256) + 256) % 256 + 1;
	        var zi = ((int)(z % 256) + 256) % 256 + 1;
	        
		    var xf = x-(int)x;
		    var yf = y-(int)y;
		    var zf = z-(int)z;

		    if (x < 0f)
		    {
			    xf += 1f;
			    xi -= 1;
		    }

		    if (y < 0f)
		    {
			    yf += 1f;
			    yi -= 1;
		    }

		    if (z < 0f)
		    {
			    zf += 1f;
			    zi -= 1;
		    }

		    var u = Misc.Math.EaseNormal(xf);
		    var v = Misc.Math.EaseNormal(yf);
		    var w = Misc.Math.EaseNormal(zf);
			
		    var a  = Permutation512[xi]+yi;
		    var aa = Permutation512[a]+zi;
		    var ab = Permutation512[a+1]+zi;
		    var b  = Permutation512[xi+1]+yi;
		    var ba = Permutation512[b]+zi;
		    var bb = Permutation512[b+1]+zi;
			
			var x1 = Misc.Math.Lerp(	
				Gradient(Permutation512[aa], xf, yf, zf),			
				Gradient(Permutation512[ba], xf-1, yf, zf),			
						u);										
			var x2 = Misc.Math.Lerp(	
				Gradient(Permutation512[ab], xf, yf-1, zf),
				Gradient(Permutation512[bb], xf-1, yf-1, zf),
						u);
			var y1 = Misc.Math.Lerp(x1, x2, v);

			x1 = Misc.Math.Lerp(	
				Gradient(Permutation512[aa+1], xf, yf, zf-1),
				Gradient(Permutation512[ba+1], xf-1, yf, zf-1),
						u);
			x2 = Misc.Math.Lerp(	
				Gradient(Permutation512[ab+1], xf, yf-1, zf-1),
				Gradient(Permutation512[bb+1], xf-1, yf-1, zf-1),
		          		u);
			var y2 = Misc.Math.Lerp (x1, x2, v);
			
			return (Misc.Math.Lerp (y1, y2, w)+1)/2;
        }

        public static float FractalBrownianMotion(float x, int octaves)
        {
	        var value = 0f;

	        var scale = 1f;
	        var accumulatedScale = 0f;
	        var frequency = 1f;
	        
			for (var o = 0; o < octaves; o++)
			{
				value += PerlinNoise(x * frequency) * scale;
				accumulatedScale += scale;
				scale /= 2;
				frequency *= 2;
			} 

	        return value / accumulatedScale;
        }
        
        public static float FractalBrownianMotion(float x, float y, int octaves)
        {
	        var value = 0f;

	        var scale = 1f;
	        var accumulatedScale = 0f;
	        var frequency = 1f;
	        
	        for (var o = 0; o < octaves; o++)
	        {
		        value += PerlinNoise(x * frequency, y * frequency) * scale;
		        accumulatedScale += scale;
		        scale /= 2;
		        frequency *= 2;
	        } 

	        return value / accumulatedScale;
        }
        
        public static float FractalBrownianMotion(float x, float y, float z, int octaves)
        {
	        var value = 0f;

	        var scale = 1f;
	        var accumulatedScale = 0f;
	        var frequency = 1f;
	        
	        for (var o = 0; o < octaves; o++)
	        {
		        value += PerlinNoise(x * frequency, y * frequency, z * frequency) * scale;
		        accumulatedScale += scale;
		        scale /= 2;
		        frequency *= 2;
	        } 

	        return value / accumulatedScale;
        }

        private static float Gradient(int hash, float x, float y, float z = 0f)
        {
            return (hash & 0xF) switch
            {
                0x0 => x + y,
                0x1 => -x + y,
                0x2 => x - y,
                0x3 => -x - y,
                0x4 => x + z,
                0x5 => -x + z,
                0x6 => x - z,
                0x7 => -x - z,
                0x8 => y + z,
                0x9 => -y + z,
                0xA => y - z,
                0xB => -y - z,
                0xC => y + x,
                0xD => -y + z,
                0xE => y - x,
                0xF => -y - z,
                _ => 0
            };
        }
    }
}