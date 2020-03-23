n = 3;
vec = [1, 2];

t = 5000000;
s = 0;
for x = 1:t
    s = s + run_scenario(n, vec);
end

s / t
%2/36 + (18/36)*(1/6)


%run_scenario(n, vec);

