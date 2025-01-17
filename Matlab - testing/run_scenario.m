function [res] = run_scenario(n, vec)
%RUN_SCENARIO Rolls dice n times to try to get fvec from ivec.
%   Detailed explanation goes here

    %ivec = [4, 4, 5, 1, 2, 3];
    %n = 10;
    %fvec = [4, 4, 5, 5, 6, 6];

    res = 0;
    found = zeros(1, length(vec));
    for x = 1:n
        roll = randi(6, 1, sum(~found));
        %roll = [3,4,3];
        
        for roll_i = 1:length(roll)
            for vec_i = 1:length(vec)
                if roll(roll_i) == vec(vec_i) && found(vec_i) == 0
                    found(vec_i) = 1;
                    break;
                end
            end
        end

        if all(found)
            res = 1;
            return;
        end
    end

end

