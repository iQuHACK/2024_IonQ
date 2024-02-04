using Bloqade
using PythonCall
using Random

plt = pyimport("matplotlib.pyplot");

nsites = 9
atoms = generate_sites(ChainLattice(), nsites, scale = 5.72)

d1, d2, d3 = 0.3, 1.6, 2.2
o1, o2, o3 = 0.05, 1.6, 2.2

Δ1 = piecewise_linear(clocks = [0.0, d1, d2, d3], values = 2π * [-20.0, -20.0, 20.0, 20.0]);
Ω1 = piecewise_linear(clocks = [0.0, o1, o2, o3], values = 2π * [0.0, 8.0, 8.0, 0.0]);
# Δ1 = piecewise_linear(clocks = [0.0, 0.3, 1.6, 2.2], values = 2π * [-10.0, -10.0, 10.0, 10.0]);
# Ω1 = piecewise_linear(clocks = [0.0, 0.05, 1.6, 2.2], values = 2π * [0.0, 4.0, 4.0, 0.0]);

Duration = 2.0

Ω2 = constant(duration = Duration, value = 2 * 2π);
Δ2 = constant(duration = Duration, value = 0);
# Ω2 = constant(duration = 2.0, value = 2 * 2π);
# Δ2 = constant(duration = 2.0, value = 0);

Ω_tot = append(Ω1, Ω2);
Δ_tot = append(Δ1, Δ2);

fig, (ax1, ax2) = plt.subplots(ncols = 2, figsize = (12, 4))
Bloqade.plot!(ax1, Ω_tot)
Bloqade.plot!(ax2, Δ_tot)
ax1.set_ylabel("Ω/2π (MHz)")
ax2.set_ylabel("Δ/2π (MHz)")
fig

h = rydberg_h(atoms; Δ = Δ_tot, Ω = Ω_tot)

reg = zero_state(nsites);

total_time = d3 + Duration;
prob = SchrodingerProblem(reg, total_time, h);
integrator = init(prob, Vern8());

entropy = Float64[]
densities = []
for _ in TimeChoiceIterator(integrator, 0.0:1e-3:total_time)
    push!(densities, rydberg_density(reg))
    rho = density_matrix(reg, (1, 2, 3, 4, 5)) # calculate the reduced density matrix
    push!(entropy, von_neumann_entropy(rho)) # compute entropy from the reduced density matrix
end

clocks = 0:1e-3:total_time
D = hcat(densities...)

fig, ax = plt.subplots(figsize = (10, 4))
shw = ax.imshow(real(D), cmap = "magma", interpolation = "nearest", aspect = "auto", extent = [0, total_time, 0.5, nsites + 0.5])
# mpl.cm.ScalarMappable(norm=mpl.colors.Normalize(0, 1), cmap='magma'), ax=ax, orientation='vertical', label='a colorbar label'
ax.set_xlabel("time (μs)")
ax.set_ylabel("site")
ax.set_xticks(0:0.4:total_time)
ax.set_yticks(1:nsites)
#colormap(f,hot);
bar = fig.colorbar(shw)
fig
