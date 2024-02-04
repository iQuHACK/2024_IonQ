using Bloqade
using PythonCall
using Random
using DelimitedFiles


plt = pyimport("matplotlib.pyplot");

A = 0

#for i in range 1:20
function createImage(i)
global duration = 5 + (0.03 * i )
 #for i in range 1:20   

nsites = 3
atoms = generate_sites(ChainLattice(), nsites, scale = 5.72)

d1, d2, d3 = 0.3, 1.6, 2.2
o1, o2, o3 = 0.05, 1.6, 2.2

Δ1 = piecewise_linear(clocks = [0.0, d1, d2, d3], values = 2π * [-30.0, -30.0, 30.0, 30.0]);
Ω1 = piecewise_linear(clocks = [0.0, o1, o2, o3], values = 2π * [0.0, 16.0, 16.0, 0.0]);
# Δ1 = piecewise_linear(clocks = [0.0, 0.3, 1.6, 2.2], values = 2π * [-10.0, -10.0, 10.0, 10.0]);
# Ω1 = piecewise_linear(clocks = [0.0, 0.05, 1.6, 2.2], values = 2π * [0.0, 4.0, 4.0, 0.0]);

#for duration in 0.4:0.4:12


Ω2 = constant(duration = duration, value = 2 * 2π);
Δ2 = constant(duration = duration, value = 0);
# Ω2 = constant(duration = 2.0, value = 2 * 2π);
# Δ2 = constant(duration = 2.0, value = 0);

Ω_tot = append(Ω1, Ω2);
Δ_tot = append(Δ1, Δ2);

fig, (ax1, ax2) = plt.subplots(ncols = 2, figsize = (15, 4))
Bloqade.plot!(ax1, Ω_tot)
Bloqade.plot!(ax2, Δ_tot)
ax1.set_ylabel("Ω/2π (MHz)")
ax2.set_ylabel("Δ/2π (MHz)")
fig

h = rydberg_h(atoms; Δ = Δ_tot, Ω = Ω_tot)

reg = zero_state(nsites);

total_time = d3 + duration;
prob = SchrodingerProblem(reg, total_time, h);
integrator = init(prob, Vern8());

entropy = Float64[]
densities = []
for _ in TimeChoiceIterator(integrator, 0.0:1e-3:total_time)
    push!(densities, rydberg_density(reg))
    rho = density_matrix(reg, (1, 2, 3)) # calculate the reduced density matrix
    push!(entropy, von_neumann_entropy(rho)) # compute entropy from the reduced density matrix
end

clocks = 0:1e-3:total_time
D = hcat(densities...)

writedlm( "densities.csv",  densities, ',')

fig, ax = plt.subplots(figsize = (20, 4))
shw = ax.imshow(real(D), cmap = "magma", interpolation = "nearest", aspect = "auto", extent = [0, total_time, 0.5, nsites + 0.5])
ax.set_xlabel("time (μs)")
ax.set_ylabel("site")
ax.set_xlim(2.4, total_time)
ax.set_xticks(2.4:0.4:total_time)
ax.set_yticks(1:nsites)
bar = fig.colorbar(shw)
fig

#i = 4
b = string(i)
file_name_a = "figurea"
file_name_intermediate = file_name_a * b
file_name_final = file_name_intermediate * ".png"
#print(i)

fig.savefig(file_name_final)
end




for i in 1:400
    global A = A + 1
    createImage(A)
end


#fig.close


