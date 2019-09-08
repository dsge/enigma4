shader_type spatial;
render_mode unshaded;


void fragment() {
    ALBEDO = vec3(1, 1, 0);
    ALPHA = dot(NORMAL, vec3(0,0,1))*1.0;
}