/*
Glowing light shader for Godot 3.0+ (GLES3)
based on the PulseGlow shader by Fernando Cosentino (https://github.com/fbcosentino/GodotPulseGlow)

Apparently only works well with smooth surfaces

Usage:
    Albedo: base object color
    
    Saturation: higher values cause a flat color in the middle with fast alpha edges
                lower values cause softer longer gradient
    
    Glow Intensity: How intense the glow is
    
    Opacity: alpha
             use this to fade whole objects in or out
*/

shader_type spatial;
render_mode blend_add, unshaded;
uniform vec4 albedo : hint_color = vec4(1.0,1.0,1.0,1.0);
uniform float Saturation : hint_range(0.5,1) = 1.0;
uniform float GlowIntensity = 0.1;
uniform float Opacity : hint_range(0,1) = 1.0;

void vertex() {
    VERTEX += NORMAL*GlowIntensity;
}

void fragment() {
    ALBEDO = albedo.rgb;
    
    float normal_dot = dot(NORMAL, vec3(0,0,1));
	normal_dot = max(normal_dot, 0.0);
	
	float arc = asin(normal_dot*Saturation)/0.785398;
	arc = pow(arc,10);
	
	float alpha_value = arc*albedo.a*Opacity;
	alpha_value = min(Opacity, alpha_value);
	ALPHA = (alpha_value);
}