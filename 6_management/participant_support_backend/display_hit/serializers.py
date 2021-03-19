from rest_framework import serializers
from .models import HITAssignment

class HITAssignmentSerializer(serializers.ModelSerializer):
    class Meta:
        model = HITAssignment
        fields = ('id', 'worker_id' , 'assignment_id','challenge','used') 
