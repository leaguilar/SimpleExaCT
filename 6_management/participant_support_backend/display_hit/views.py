from django.shortcuts import render
from rest_framework import viewsets
from .serializers import HITAssignmentSerializer
from .models import HITAssignment

from rest_framework.authentication import SessionAuthentication, BasicAuthentication, TokenAuthentication
from rest_framework.permissions import IsAuthenticated
from rest_framework.response import Response
from rest_framework.views import APIView


# Create your views here.
class HITAssignmentView(viewsets.ModelViewSet):
    authentication_classes = [SessionAuthentication, BasicAuthentication, TokenAuthentication]
    permission_classes = [IsAuthenticated]
    serializer_class = HITAssignmentSerializer
    queryset = HITAssignment.objects.all()


class HITAssignmentServe(APIView):
    experiment_name = "ProofOfConcept1"

    def post(self, request, format=None):
        print(request)
        if f'{self.experiment_name}_challenge_id' in request.COOKIES:
            print("Already seen")
            challenge_id = request.COOKIES[f'{self.experiment_name}_challenge_id']
            group = request.COOKIES[f'{self.experiment_name}_group']
            content = {
                'challenge': str(challenge_id),
                'group': str(group)
            }
            response = Response(content)
            return response
        else:
            print("First time seen")
            if request.data['workerId'] and request.data['hitId'] and request.data['assignmentId']:
                assignment = HITAssignment.objects.filter(used=False).first()
                if assignment:
                    workerId = request.data['workerId']
                    hitId = request.data['hitId']
                    assignmentId = request.data['assignmentId']
                    # assignment.update(used=True, worker_id=workerId, assignment_id=assignmentId, hit_id=hitId)
                    assignment.used = True
                    assignment.worker_id = workerId
                    assignment.assignment_id = assignmentId
                    assignment.hit_id = hitId
                    assignment.save()
                    content = {
                        'challenge': str(assignment.challenge),
                        'group': str(assignment.group)
                    }
                    response = Response(content)
                    response.set_cookie(f'{self.experiment_name}_challenge_id', assignment.challenge)
                    response.set_cookie(f'{self.experiment_name}_group', assignment.group)
                    return response
            content = {
                'challenge': '',
                'group': ''
            }
            response = Response(content)
            return response
